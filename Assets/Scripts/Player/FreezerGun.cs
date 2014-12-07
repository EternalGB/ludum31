using UnityEngine;
using System.Collections;

public class FreezerGun : MonoBehaviour
{

	public ParticleSystem particles;
	public GameObject freezerZone;
	public float ammo;
	public float maxAmmo;
	public bool CanFire
	{
		get{return ammo > 0;}
	}

	public void Fire(Vector3 target)
	{
		if(ammo > 0) {
			particles.Play();
			particles.enableEmission = true;
			freezerZone.SetActive(true);
			Util.FaceTarget(transform,target);
			ammo -= 1;
		}
	}

	public void StopFire()
	{
		particles.Stop();
		particles.enableEmission = false;

		freezerZone.SetActive(false);
	}

	public void ReceiveAmmo(float amount)
	{
		ammo = Mathf.Clamp(ammo + amount,0,maxAmmo);
	}

}

