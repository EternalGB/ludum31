using UnityEngine;
using System.Collections;

public class BorderAnim : MonoBehaviour
{

	public float fill;
	public float maxSize;
	Vector3 scale;

	void Start()
	{
		scale = new Vector3(maxSize,maxSize,1);
	}

	void Update()
	{
		transform.localScale = scale*fill;
	}


}

