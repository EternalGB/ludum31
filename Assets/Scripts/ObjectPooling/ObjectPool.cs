using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{



	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool growable = true;

	List<GameObject> pool;
	
	public void Init(GameObject pooledObject, int pooledAmount, bool growable) 
	{
		pool = new List<GameObject>();
		this.pooledObject = pooledObject;
		this.pooledAmount = pooledAmount;
		this.growable = growable;
		for(int i = 0; i < pooledAmount; i++) {
			pool.Add(GetNewObject());
		}
	}

	public GameObject GetPooled()
	{
		for(int i = 0; i < pool.Count; i++) {
			if(pool[i] && !pool[i].activeInHierarchy) {
				return pool[i];
			}
		}

		if(growable) {
			GameObject obj = GetNewObject();
			pool.Add(obj);
			return obj;
		}

		return null;
	}

	public bool ObjectAvailable()
	{
		for(int i = 0; i < pool.Count; i++) {
			if(pool[i] && !pool[i].activeInHierarchy) {
				return true;
			}
		}
		return false;
	}

	GameObject GetNewObject()
	{
		GameObject obj = (GameObject)Instantiate(pooledObject);
		obj.name = pooledObject.name;
		obj.SetActive(false);
		return obj;
	}

}

