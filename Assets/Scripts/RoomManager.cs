using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{

	public Room[] rooms;
	public float roomDestructionTime;
	public GameObject playerRoom;
	public List<GameObject> emptyRoomPrefabs;
	public List<GameObject> blueRoomPrefabs;
	public List<GameObject> greenRoomPrefabs;
	public List<GameObject> purpleRoomPrefabs;
	public List<GameObject> redRoomPrefabs;

	public GameObject ammoPickup;
	public GameObject healthPickup;
	public GameObject freezerPickup;
	
	public float purpleRoomChance, purpleChanceIncreaseRate, purpleChanceMax;

	int[] activatedRooms;
	int numActivated = 0;
	int matchNum = 3;
	Dictionary<int,List<int>> roomAdjacencies;
	Pathfinder pathfinder;
	bool sliding = false;
	float slidingLerpTimer = 0;
	float slidingLerpSpeed = 0.5f;

	int numRoomsSpawned = 0;

	Dictionary<GameObject,Vector3> otherSlidingObjects;

	void Start()
	{
		pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();
		roomAdjacencies = new Dictionary<int, List<int>>();
		for(int i = 0; i < rooms.Length; i++) {
			List<int> adjRooms = GetAdjacentRooms(i);
			roomAdjacencies.Add(i,adjRooms);
			/*
			string message = "Rooms adjacent to " + i + " : ";
			foreach(int j in adjRooms)
				message += " " + j;
			Debug.Log (message);
			*/
			LinkRoomNavs(i);
		}

		foreach(Room room in rooms) {
			room.RoomActivated += HandleRoomActivation;
		}
	}

	void Update()
	{
		if(sliding) {
			//if room is not in correct position, slide it a bit
			for(int i = 0; i < rooms.Length; i++) {
				Vector3 correctPos = RoomPosToWorldPos(i);
				if(rooms[i].transform.position != correctPos)
					rooms[i].transform.position = Vector3.Lerp(rooms[i].transform.position,correctPos,slidingLerpTimer*slidingLerpSpeed);
			}
			//slide everything else down that needs sliding also
			foreach(GameObject obj in otherSlidingObjects.Keys) {
				obj.transform.position = Vector3.Lerp(obj.transform.position,otherSlidingObjects[obj],slidingLerpTimer*slidingLerpSpeed);
			}
			slidingLerpTimer = Mathf.Clamp(slidingLerpTimer + Time.unscaledDeltaTime,0f,1f);
			if(slidingLerpTimer == 1)
				EndSliding();
		}
	}

	void HandleRoomActivation (Room room)
	{
		numActivated++;
		if(numActivated == matchNum) {
			if(CheckForMatches()) {
				StartRoomClear();
				RoomType matchType = GetTypeMatch();
				switch(matchType) {
				case RoomType.NONE:
					//do nothing
					break;
				case RoomType.BLUE:
					//spawn the freezer powerup
					Util.GetRandomElement<Room>(rooms).SpawnPickup(freezerPickup);
					break;
				case RoomType.GREEN:
					//spawn some ammo
					Util.GetRandomElement<Room>(rooms).SpawnPickup(ammoPickup);
					break;
				case RoomType.PURPLE:
					//win!
					break;
				case RoomType.RED:
					//spawn some health
					Util.GetRandomElement<Room>(rooms).SpawnPickup(healthPickup);
					break;
				}
			} else {
				foreach(Room r in rooms) {
					r.Deactivate();
				}
			}
			numActivated = 0;
		}
	}

	void StartRoomClear()
	{
		Debug.Log ("StartRoomClear");
		foreach(Room room in rooms) {
			if(room.activated)
				room.StartDestroy();
		}
		Invoke("ClearAndSpawnNewRooms",roomDestructionTime);
	}

	void ClearAndSpawnNewRooms()
	{
		Debug.Log ("Starting clear and new rooms");
		//Time.timeScale = 0;
		for(int i = 0; i < rooms.Length; i++) {
			UnlinkRoomNavs(i);
		}
		Room[] deletedRooms = new Room[3];
		//activated rooms should still be there
		for(int i = 0; i < activatedRooms.Length; i++) {
			deletedRooms[i] = rooms[activatedRooms[i]];
			rooms[activatedRooms[i]] = null;
		}

		//check if there's anything in a destruction room
		foreach(Room room in deletedRooms) {
			List<GameObject> objs = room.GetObjectsInside();
			foreach(GameObject obj in objs) {
				if(obj.layer == LayerMask.NameToLayer("Player")) {
					//TODO GAME OVER
				} else {
					GameObject.DestroyImmediate(obj);
				}
			}
		}

		//slide existing rooms down (virtually) into their positions in the array
		for(int i = 0; i < rooms.Length; i++) {
			if(rooms[i] == null) {
				int cursor = i;
				while(cursor < 9 && rooms[cursor] == null) {
					cursor += 3;
				}
				if(cursor < 9 && rooms[cursor] != null) {
					rooms[i] = rooms[cursor];
					rooms[cursor] = null;
				}
			}
		}
		//make new rooms in the positions they should be in but off camera
		Vector2[] positions = new Vector2[3];
		int posCounter = 0;
		for(int i = 0; i < rooms.Length; i++) {
			if(rooms[i] == null) {
				positions[posCounter] = new Vector2(16*(i%3)+8, 16*(i/3)+8) + (new Vector2(0,16*3)); //to push above map
				GameObject roomObj = (GameObject)GameObject.Instantiate(SelectNextRoom(),positions[posCounter],Quaternion.identity);
				rooms[i] = roomObj.GetComponent<Room>();
				rooms[i].RoomActivated += HandleRoomActivation;
				posCounter++;
				numRoomsSpawned++;
			}
		}



		//delete old rooms
		for(int i = 0; i < deletedRooms.Length; i++) {
			deletedRooms[i].RoomActivated -= HandleRoomActivation;
			GameObject.DestroyImmediate(deletedRooms[i].gameObject);
		}

		LinkAllRoomNavs();
		pathfinder.ResetPointList();


		//find any objects inside the room that need sliding and calculate their correct position
		otherSlidingObjects = new Dictionary<GameObject, Vector3>();
		for(int i = 0; i < rooms.Length; i++) {
			Room room = rooms[i];
			if(rooms[i].transform.position != RoomPosToWorldPos(i)) {
				List<GameObject> objs = room.GetObjectsInside();
				foreach(GameObject obj in objs) {
					//apparently things can be in two rooms at once :|
					if(!otherSlidingObjects.ContainsKey(obj))
						otherSlidingObjects.Add(obj,obj.transform.position + (RoomPosToWorldPos(i) - rooms[i].transform.position));
				}
			}
		}

		//slide everything down (with animation)
		sliding = true;
		slidingLerpTimer = 0;
		//StartCoroutine(Timers.CountdownRealtime(1/slidingLerpSpeed,EndSliding));


	}

	GameObject SelectNextRoom()
	{
		//the first few rooms are special
		if(numRoomsSpawned == 0)
			return playerRoom;
		else if(numRoomsSpawned < 3) 
			return Util.GetRandomElement(emptyRoomPrefabs);
		else if(Random.value <= purpleRoomChance) {
			return Util.GetRandomElement(purpleRoomPrefabs);
		} else {
			purpleRoomChance = Mathf.Clamp(purpleRoomChance + purpleChanceIncreaseRate,0,purpleChanceMax);
			float roll = Random.value;
			if(roll < 0.33)
				return Util.GetRandomElement(blueRoomPrefabs);
			else if(roll < 0.66)
				return Util.GetRandomElement(greenRoomPrefabs);
			else
				return Util.GetRandomElement(redRoomPrefabs);
		}
	}

	void EndSliding()
	{
		sliding = false;
	}

	bool CheckForMatches()
	{
		activatedRooms = new int[3];
		int roomIndex = 0;
		for(int i = 0; i < rooms.Length; i++) {
			if(rooms[i].activated) {
				activatedRooms[roomIndex] = i;
				roomIndex++;
			}
		}
		foreach(int i in activatedRooms) {
			bool adjActivated = false;
			foreach(int j in roomAdjacencies[i]) {
				if(rooms[j].activated)
					adjActivated = true;
			}
			if(!adjActivated)
				return false;
		}
		return true;
	}

	RoomType GetTypeMatch()
	{
		RoomType type = RoomType.NONE;
		if(rooms[activatedRooms[0]].type == rooms[activatedRooms[1]].type &&
		   rooms[activatedRooms[0]].type == rooms[activatedRooms[2]].type)
			type = rooms[activatedRooms[0]].type;
		return type;
	}

	List<int> GetAdjacentRooms(int roomIndex)
	{
		List<int> adjRooms = new List<int>();
		if(roomIndex + 1 < 9 && (roomIndex+1)%3 != 0)
			adjRooms.Add(roomIndex+1);
		if(roomIndex + 3 < 9)
			adjRooms.Add(roomIndex+3);
		if(roomIndex - 1 >= 0 && (roomIndex)%3 != 0)
			adjRooms.Add (roomIndex-1);
		if(roomIndex - 3 >= 0)
			adjRooms.Add(roomIndex-3);
		return adjRooms;
	}

	void LinkAllRoomNavs()
	{
		for(int i = 0; i < rooms.Length; i++)
			LinkRoomNavs(i);
	}

	void LinkRoomNavs(int roomIndex)
	{
		foreach(int j in roomAdjacencies[roomIndex]) {
			LinkRoomNavs(rooms[roomIndex],rooms[j]);
		}
	}

	void LinkRoomNavs(Room r1, Room r2)
	{
		NavPoint[] nps1 = r1.GetComponentsInChildren<NavPoint>();
		NavPoint[] nps2 = r2.GetComponentsInChildren<NavPoint>();
		if(r2.transform.position.x > r1.transform.position.x) {
			NavPoint np1 = GetNamedPoint("DR",nps1);
			NavPoint np2 = GetNamedPoint("DL",nps2);
			np1.neighbours.Add(np2);
			//np2.neighbours.Add(np1);
		} else if(r2.transform.position.x < r1.transform.position.x) {
			NavPoint np1 = GetNamedPoint("DL",nps1);
			NavPoint np2 = GetNamedPoint("DR",nps2);
			np1.neighbours.Add(np2);
			//np2.neighbours.Add(np1);
		} else if(r2.transform.position.y > r1.transform.position.y) {
			NavPoint np1 = GetNamedPoint("DU",nps1);
			NavPoint np2 = GetNamedPoint("DD",nps2);
			np1.neighbours.Add(np2);
			//np2.neighbours.Add(np1);
		} else if(r2.transform.position.y < r1.transform.position.y) {
			NavPoint np1 = GetNamedPoint("DD",nps1);
			NavPoint np2 = GetNamedPoint("DU",nps2);
			np1.neighbours.Add(np2);
			//np2.neighbours.Add(np1);
		}
	}

	NavPoint GetNamedPoint(string name, NavPoint[] nps)
	{
		NavPoint np = null;
		foreach(NavPoint nav in nps) {
			if(nav.gameObject.name == name)
				np = nav;
		}
		return np;
	}

	void UnlinkRoomNavs(int roomIndex)
	{
		foreach(int j in roomAdjacencies[roomIndex]) {
			UnlinkRoomNavs(rooms[roomIndex],rooms[j]);
		}
	}

	void UnlinkRoomNavs(Room r1, Room r2)
	{
		if(r1 == null)
			return;
		else if(r2 == null) {
			NavPoint[] nps = r1.GetComponentsInChildren<NavPoint>();
			foreach(NavPoint np in nps)
				np.RemoveDeadNeighbours();
			return;
		}
		NavPoint[] nps1 = r1.GetComponentsInChildren<NavPoint>();
		NavPoint[] nps2 = r2.GetComponentsInChildren<NavPoint>();
		if(r2.transform.position.x > r1.transform.position.x) {
			NavPoint np1 = GetNamedPoint("DR",nps1);
			NavPoint np2 = GetNamedPoint("DL",nps2);
			np1.neighbours.Remove(np2);
			//np2.neighbours.Remove(np1);
		} else if(r2.transform.position.x < r1.transform.position.x) {
			NavPoint np1 = GetNamedPoint("DL",nps1);
			NavPoint np2 = GetNamedPoint("DR",nps2);
			np1.neighbours.Remove(np2);
			//np2.neighbours.Remove(np1);
		} else if(r2.transform.position.y > r1.transform.position.y) {
			NavPoint np1 = GetNamedPoint("DU",nps1);
			NavPoint np2 = GetNamedPoint("DD",nps2);
			np1.neighbours.Remove(np2);
			//np2.neighbours.Remove(np1);
		} else if(r2.transform.position.y < r1.transform.position.y) {
			NavPoint np1 = GetNamedPoint("DD",nps1);
			NavPoint np2 = GetNamedPoint("DU",nps2);
			np1.neighbours.Remove(np2);
			//np2.neighbours.Remove(np1);
		}
	}

	public static Vector3 RoomPosToWorldPos(int roomPos)
	{
		return new Vector3((roomPos%3)*16+8,(roomPos/3)*16+8);
	}

}

