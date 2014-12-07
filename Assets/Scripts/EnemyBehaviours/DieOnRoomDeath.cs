using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameActor))]
public class DieOnRoomDeath : MonoBehaviour
{

	public Room homeRoom;

	void Update()
	{
		if(homeRoom == null)
			SendMessage("Die");
	}
}

