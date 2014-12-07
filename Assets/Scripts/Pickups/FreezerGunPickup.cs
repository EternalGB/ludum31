using UnityEngine;
using System.Collections;

public class FreezerGunPickup : Pickup
{

	public float ammoValue;

	public override void ApplyPickup (GameObject target)
	{
		PlayerController pc = target.GetComponent<PlayerController>();
		if(!pc.haveFreezerGun)
			pc.haveFreezerGun = true;
		pc.ReceiveFreezerAmmo(ammoValue);
	}

}

