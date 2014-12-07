using UnityEngine;
using System.Collections;

public class AmmoPickup : Pickup
{

	public float ammoValue;

	public override void ApplyPickup (GameObject target)
	{
		PlayerController pc = target.GetComponent<PlayerController>();
		pc.ReceiveAmmo(ammoValue);
	}

}

