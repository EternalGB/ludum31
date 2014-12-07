using UnityEngine;
using System.Collections;

public class DamageOnCollision : MonoBehaviour
{

	public float damagePerSecond;
	public LayerMask targets;

	void OnCollisionStay2D(Collision2D coll)
	{
		if((targets.value &1 << coll.gameObject.layer) != 0) {
			GameActor ga;
			if(ga = coll.gameObject.GetComponent<GameActor>())
				ga.TakeDamage(damagePerSecond*Time.deltaTime);
		}
	}

}

