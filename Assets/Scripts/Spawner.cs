using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{

	public GameObject prefab;
	public float spawnRate;
	public Transform spawnPos;

	public float maxSize;
	float lerpTimer = 0;
	float lerpSpeed;
	Vector3 scale;

	ObjectPool pool;

	void OnEnable()
	{
		//wanted faster spawns and couldn't be bothered going through all the prefabs
		spawnRate = spawnRate*1.5f;
		pool = PoolManager.Instance.GetPoolByRepresentative(prefab);
		Invoke("Spawn",1/spawnRate);
		lerpSpeed = spawnRate;
		scale = new Vector3(maxSize,maxSize,1);

	}

	void Update()
	{
		transform.localScale = Vector3.Lerp(Vector3.one,scale,lerpTimer);
		lerpTimer = Mathf.Clamp(lerpTimer + Time.deltaTime*lerpSpeed,0,1);
	}
	

	void Spawn()
	{
		lerpTimer = 0;
		transform.localScale = Vector3.one;
		GameObject obj = pool.GetPooled();
		obj.SetActive(true);
		obj.transform.position = spawnPos.position;
		Invoke("Spawn",1/spawnRate);
	}

	void OnDisable()
	{
		lerpTimer = 0;
		transform.localScale = Vector3.one;
		CancelInvoke("Spawn");
	}

}

