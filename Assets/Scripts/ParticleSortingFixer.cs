using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ParticleSortingFixer : MonoBehaviour 
{

	public string sortingLayerName;

	void Start()
	{
		particleSystem.renderer.sortingLayerName = sortingLayerName;
	}

}
