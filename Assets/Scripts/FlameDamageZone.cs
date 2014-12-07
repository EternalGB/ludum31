using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class FlameDamageZone : MonoBehaviour
{

	public float damagePerSecond;
	public LayerMask targets;

	void OnTriggerStay2D(Collider2D coll) 
	{
		if((targets.value &1 << coll.gameObject.layer) != 0) {
			GameActor ga;
			if(ga = coll.gameObject.GetComponent<GameActor>())
				ga.TakeDamage(damagePerSecond*Time.deltaTime);
		}
	}

}

