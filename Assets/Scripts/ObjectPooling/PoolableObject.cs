using UnityEngine;
using System.Collections;

public abstract class PoolableObject : MonoBehaviour
{

	public virtual void Destroy()
	{
		gameObject.SetActive (false);
	}

}

