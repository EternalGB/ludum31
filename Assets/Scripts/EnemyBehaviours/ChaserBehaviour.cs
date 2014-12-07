using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameActor))]
public class ChaserBehaviour : MonoBehaviour
{

	GameActor ga;
	Transform target;
	Pathfinder pathfinder;
	public LayerMask rayMask;

	void OnEnable()
	{
		target = GameObject.Find ("Player").transform;
		pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();
		ga = GetComponent<GameActor>();
	}



	void Update()
	{
		if(target != null) {
			//if we can see our target then just run at it
			//otherwise we need to path around stuff
			RaycastHit2D hit;
			Vector3 targetDir = (target.position - transform.position).normalized;
			if(hit = Physics2D.Raycast(transform.position,targetDir,100,rayMask)) {
				Debug.DrawRay(transform.position,targetDir,Color.red);
				if(hit.transform.gameObject.layer == target.gameObject.layer) {
					ga.moveDir = targetDir;
				} else {
					//find the path and pick the next appropriate point along it to follow
					NavPoint[] path = pathfinder.GetShortestPath(pathfinder.ClosestNavPoint(transform.position),target);
					if(path != null) {
						//Debug.Log ("Got Path");
						//Pathfinder.LogPath(path);
						NavPoint nextPos = path[0];
						int i = 0;
						while(i < path.Length && Vector3.Distance(path[i].position,transform.position) <= 5) {
							nextPos = path[i];
							i++;
						}
						//Debug.Log ("Next pos is " + nextPos.position);
						ga.moveDir = (nextPos.position - transform.position).normalized;
						
					} else {
						ga.moveDir = Vector2.zero;
					}
				}
				
			}
			
				

		}

		rigidbody2D.angularVelocity = Mathf.Sign(rigidbody2D.velocity.x)*rigidbody2D.velocity.magnitude*50;
	}

		
}

