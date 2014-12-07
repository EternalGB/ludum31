using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{

	public float ammo;
	public float ammoConsumedPerShot;
	public float attackSpeed;
	bool canFire = true;

	public void Fire(Vector3 target)
	{
		if(canFire && ammo - ammoConsumedPerShot > 0) {
			OnFire(target);
			ammo -= ammoConsumedPerShot;
			canFire = false;
			StartCoroutine(Timers.Countdown(1/attackSpeed,() => canFire = true));
		}
	}


	protected abstract void OnFire(Vector3 target);

}

