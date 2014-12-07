using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FillBarController : MonoBehaviour
{

	public float value;
	public float max;
	Image image;

	void Start()
	{
		image = GetComponent<Image>();
	}

	void Update()
	{
		if(max > 0)
			image.fillAmount = value/max;
	}
		
}

