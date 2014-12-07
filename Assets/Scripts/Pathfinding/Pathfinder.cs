using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{

	public List<NavPoint> allPoints;
	public LayerMask rayMask;

	void Start()
	{
		ResetPointList();
	}

	public NavPoint[] GetShortestPath(NavPoint start, Transform target)
	{
		HashSet<NavPoint> visited = new HashSet<NavPoint>();
		SortedDictionary<float,NavPoint> pq = new SortedDictionary<float, NavPoint>();
		Dictionary<NavPoint,NavPoint> cameFrom = new Dictionary<NavPoint, NavPoint>();
		Dictionary<NavPoint,float> scores = new Dictionary<NavPoint, float>();


		pq.Add(Vector3.Distance(target.position,start.position),start);
		scores.Add(start,Vector3.Distance(target.position,start.position));

		while(pq.Count > 0) {
			SortedDictionary<float,NavPoint>.KeyCollection.Enumerator keyEnumer = pq.Keys.GetEnumerator();
			keyEnumer.MoveNext();
			NavPoint np = pq[keyEnumer.Current];
			pq.Remove(keyEnumer.Current);

			if(IsGoal(np, target))
				return ReconstructPath(cameFrom,np);

			visited.Add(np);
			foreach(NavPoint neighbour in np.neighbours) {
				if(neighbour.gameObject.activeInHierarchy) {
					if(visited.Contains(neighbour))
						continue;
					float estimate = scores[np] + Vector3.Distance(np.position,neighbour.position);
					//if this is the first time we've seen this node
					//or if we have a found a shorter path to it
					if(!pq.ContainsValue(neighbour) || (scores.ContainsKey(neighbour) && estimate < scores[neighbour])) {
						cameFrom[neighbour] = np;
						scores[neighbour] = estimate;
						try {
							pq.Add(estimate + Vector3.Distance(neighbour.position,target.position),neighbour);
						} catch (System.ArgumentException e) {
							//this means the key already exists i.e. there is already a distance that is this value
							//this is real bad for A* that we can't have two things of the same priority but I'm just going
							//to ignore it because GAME JAM
							pq.Add(estimate + Vector3.Distance(neighbour.position,target.position)+Random.value/100,neighbour);
						}
					}
				}
			}
		}

		return null;
	}

	NavPoint[] ReconstructPath(Dictionary<NavPoint,NavPoint> cameFrom, NavPoint current)
	{
		Stack<NavPoint> path = new Stack<NavPoint>();
		path.Push(current);
		while(cameFrom.ContainsKey(current)) {
			current = cameFrom[current];
			path.Push(current);
		}
		return path.ToArray();
	}

	//a navpoint is the goal if we can see the target from it
	bool IsGoal(NavPoint np, Transform target)
	{
		RaycastHit2D hit;
		Vector3 targetDir = (target.position - np.position).normalized;
		if(hit = Physics2D.Raycast(np.position,targetDir,100,rayMask)) {
			return hit.transform.gameObject.layer == target.gameObject.layer;
		}
		return false;
	}

	public NavPoint ClosestNavPoint(Vector3 position)
	{
		float bestDist = float.MaxValue;
		float dist = 0;

		//start with the first active navpoint we can find
		NavPoint current = allPoints[0];;
		foreach(NavPoint np in allPoints) {
			if(np.gameObject.activeInHierarchy) {
				current = np;
				break;
			}
		}

		foreach(NavPoint np in allPoints) {
			if(np != null && np.gameObject.activeInHierarchy) {
				dist = Vector3.Distance(position,np.position);
				if(dist < bestDist) {
					current = np;
					bestDist = dist;
				}
			}
		}
		return current;
	}

	public static void LogPath(NavPoint[] points)
	{
		string message = "Path: ";
		foreach(NavPoint np in points) {
			message += " " + np.position;
		}
		Debug.Log(message);
	}

	public void ResetPointList()
	{
		allPoints = new List<NavPoint>();
		GameObject[] pointObjs = GameObject.FindGameObjectsWithTag("NavPoint");
		foreach(GameObject pointObj in pointObjs) {
			allPoints.Add(pointObj.GetComponent<NavPoint>());
		}
	}

}

