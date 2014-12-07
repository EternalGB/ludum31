using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject playerDummyPrefab;
	public RoomManager roomManager;

	public GameObject gameWinUI;

	void Start()
	{

		StartCoroutine(Timers.Countdown<int>(2,ActivateRoom,0));
		StartCoroutine(Timers.Countdown<int>(3,ActivateRoom,1));
		StartCoroutine(Timers.Countdown<int>(4,ActivateRoom,2));
	}


	void ActivateRoom(int i)
	{
		GameObject dummy = (GameObject)GameObject.Instantiate(playerDummyPrefab,RoomManager.RoomPosToWorldPos(i),Quaternion.identity);
		StartCoroutine(Timers.Countdown<GameObject>(2,DestroyObject,dummy));
	}

	void DestroyObject(GameObject obj)
	{
		DestroyImmediate(obj);
	}

	public void GameOver()
	{
		Restart();
	}

	public void GameWin()
	{
		Time.timeScale = 0;
		gameWinUI.SetActive(true);
	}

	public void Restart()
	{
		Time.timeScale = 1;
		Application.LoadLevel(Application.loadedLevel);
	}

}

