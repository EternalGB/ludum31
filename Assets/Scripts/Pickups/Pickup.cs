using UnityEngine;
using System.Collections;

public abstract class Pickup : MonoBehaviour
{
	
	public LayerMask allowedToPickup;
	public float spinSpeed;
	public AudioClip pickupSound;

	public abstract void ApplyPickup(GameObject target);

	void Update()
	{
		transform.Rotate(Vector3.forward,spinSpeed*Time.deltaTime);
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		if((allowedToPickup.value &1 << coll.gameObject.layer) != 0) {
			audio.PlayOneShot(pickupSound);
			ApplyPickup(coll.gameObject);
			GameObject.Destroy(gameObject);
		}
	}

	void Die()
	{
		GameObject.Destroy(gameObject);
	}

}

