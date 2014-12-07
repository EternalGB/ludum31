using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GameActor))]
public class GuardianBehaviour : MonoBehaviour
{

	GameActor ga;
	GameObject target;
	public Room room;
	public Transform home;
	public LayerMask rayMask;

	void OnEnable()
	{
		target = GameObject.Find ("Player");
		ga = GetComponent<GameActor>();
	}
	
	void Update()
	{
		if(target != null) {
			List<GameObject> inRoom = room.GetObjectsInside();
			if(inRoom.Contains(target)) {
				//if we can see our target then just run at it
				RaycastHit2D hit;
				Vector3 targetDir = (target.transform.position - transform.position).normalized;
				if(hit = Physics2D.Raycast(transform.position,targetDir,100,rayMask)) {
					Debug.DrawRay(transform.position,targetDir,Color.red);
					if(hit.transform.gameObject.layer == target.gameObject.layer) {
						ga.moveDir = targetDir;
					}
				}
			} else {
				//go back to our starting position
				if(Vector3.Distance(home.position,transform.position) > 0.5)
					ga.moveDir = (home.position - transform.position).normalized;
				else {
					ga.moveDir = Vector2.zero;
					transform.rotation = Quaternion.identity;
				}
			}
		}
		
		rigidbody2D.angularVelocity = Mathf.Sign(rigidbody2D.velocity.x)*rigidbody2D.velocity.magnitude*50;
	}

	void Die()
	{
		GameObject.DestroyImmediate(gameObject);
	}

}

