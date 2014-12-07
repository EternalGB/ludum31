using UnityEngine;
using System.Collections.Generic;

public class NavGrid : MonoBehaviour
{

	public List<NavPoint> points;



	public void MakeSymmetrical()
	{
		foreach(NavPoint np in points) {
			foreach(NavPoint neighbour in np.neighbours) {
				if(!neighbour.neighbours.Contains(np))
					neighbour.neighbours.Add(np);
			}
		}
	}

}

