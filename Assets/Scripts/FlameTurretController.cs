using UnityEngine;
using System.Collections;

public class FlameTurretController : MonoBehaviour
{

	public ParticleSystem particles;
	public GameObject damageZone;
	public float activeTime;
	public float inactiveTime;
	public float startDelay;

	void Start()
	{
		damageZone.SetActive(false);
		Invoke("Fire",startDelay);
	}

	void Fire()
	{
		damageZone.SetActive(true);
		particles.Play();
		Invoke("StopFire",activeTime);
	}

	void StopFire()
	{
		damageZone.SetActive(false);
		particles.Stop();
		Invoke("Fire",inactiveTime);
	}



}

