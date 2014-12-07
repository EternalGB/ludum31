using UnityEngine;
using System.Collections.Generic;

public class NavPoint : MonoBehaviour
{

	public List<NavPoint> neighbours;
	public Vector3 position
	{
		get {return transform.position;}
		set {transform.position = value;}
	}

	public void RemoveDeadNeighbours()
	{
		List<NavPoint> nps = new List<NavPoint>();
		foreach(NavPoint np in neighbours)
			if(np != null)
				nps.Add(np);
		neighbours = nps;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position,0.5f);
		if(neighbours != null) {
			foreach(NavPoint t in neighbours) {

				Gizmos.DrawLine(transform.position,t.transform.position);
			}
		}
	}

}

