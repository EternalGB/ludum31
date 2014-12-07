using UnityEngine;
using System.Collections;

public class DetachFromRoom : MonoBehaviour
{

	void Start()
	{
		transform.parent = null;
	}

}

