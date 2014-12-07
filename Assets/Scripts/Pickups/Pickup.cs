using UnityEngine;
using System.Collections;

public abstract class Pickup : MonoBehaviour
{
	
	public LayerMask allowedToPickup;
	public float spinSpeed;

	public abstract void ApplyPickup(GameObject target);

	void Update()
	{
		transform.Rotate(Vector3.forward,spinSpeed*Time.deltaTime);
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		if((allowedToPickup.value &1 << coll.gameObject.layer) != 0) {
			ApplyPickup(coll.gameObject);
			GameObject.Destroy(gameObject);
		}
	}

}

