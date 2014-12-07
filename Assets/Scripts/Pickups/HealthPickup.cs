using UnityEngine;
using System.Collections;

public class HealthPickup : Pickup
{

	public float healthValue;

	public override void ApplyPickup (GameObject target)
	{
		target.GetComponent<GameActor>().Heal(healthValue);
	}

}

