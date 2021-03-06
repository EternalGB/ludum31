using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
	
	public RoomType type;
	public CrystalController cc;
	public bool activated;
	public Vector2 roomSize;
	public Vector2 floorSize;
	public LayerMask actorsMask;

	public delegate void RoomActivatedEvent(Room room);
	public event RoomActivatedEvent RoomActivated;



	public void Activate()
	{
		activated = true;
		if(RoomActivated != null)
			RoomActivated(this);
	}

	public void Deactivate()
	{
		cc.Deactivate();
		activated = false;
	}

	public void StartDestroy()
	{
		cc.StartDestroy();
	}
	
	public List<GameObject> GetObjectsInside()
	{
		List<GameObject> objs = new List<GameObject>();
		Vector2 topLeft = ((Vector2)transform.position) - roomSize/2;
		Vector2 bottomRight = topLeft + roomSize;
		Collider2D[] colls = Physics2D.OverlapAreaAll(topLeft,bottomRight,actorsMask);
		if(colls != null) {
			foreach(Collider2D coll in colls) {
				objs.Add(coll.gameObject);
			}
		}
		return objs;
	}

	public void SpawnInsideRoom(GameObject prefab)
	{
		Vector2 topLeft = ((Vector2)transform.position) - floorSize/2;
		Vector2 bottomRight = topLeft + floorSize;
		Vector3 position = Util.RandomPointInside(topLeft,bottomRight);
		GameObject go = (GameObject)GameObject.Instantiate(prefab,position,Quaternion.identity);
		go.name = prefab.name;
	}

	public void RespawnSentinal(GameObject prefab, float waitTime)
	{
		GameObject toSpawn = prefab;
		float time = waitTime;
		StartCoroutine(Timers.Countdown<GameObject>(time,RespawnSentinal,toSpawn));
	}

	void RespawnSentinal(GameObject prefab)
	{
		GameObject go = (GameObject)GameObject.Instantiate(prefab,transform.position,Quaternion.identity);
		go.name = prefab.name;
		go.GetComponent<DieOnRoomDeath>().homeRoom = this;
		go.GetComponent<SentinalBehaviour>().homeRoom = this;
	}

		
}

