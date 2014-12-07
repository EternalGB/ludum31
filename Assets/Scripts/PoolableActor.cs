using UnityEngine;
using System.Collections;

public class PoolableActor : PoolableObject
{

	void Die()
	{
		Destroy();
	}

}

