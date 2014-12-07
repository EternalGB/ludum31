using UnityEngine;
using System.Collections;

public class SentinalBehaviour : MonoBehaviour
{

	public GameObject sentinalPrefab;
	public Room homeRoom;
	public float respawnTime;

	void Die()
	{
		if(homeRoom != null) {
			homeRoom.RespawnSentinal(sentinalPrefab,respawnTime);
		}
		GameObject.Destroy(gameObject);
	}

}

