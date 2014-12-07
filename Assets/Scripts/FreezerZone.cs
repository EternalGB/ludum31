using UnityEngine;
using System.Collections;

public class FreezerZone : MonoBehaviour
{

	public float freezePerSecond;
	public LayerMask targets;

	void OnTriggerStay2D(Collider2D coll) 
	{
		if((targets.value &1 << coll.gameObject.layer) != 0) {
			GameActor ga;
			if(ga = coll.gameObject.GetComponent<GameActor>())
				ga.TakeCold(freezePerSecond*Time.deltaTime);
		}
	}

}

