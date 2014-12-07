using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameActor))]
public class PlayerController : MonoBehaviour 
{

	GameActor ga;
	public LineRenderer lr;
	ParticleSystem hitParticles;
	public UnscaledTimeParticleController deathParticles;
	public float ammo;
	public float maxAmmo;
	public float attackSpeed;
	public float damage;
	bool canFire = true;
	public LayerMask shootable;
	float fireLineVisibilityTime = 0.04f;

	public FreezerGun freezerGun;
	public bool haveFreezerGun;

	FillBarController healthBar;
	FillBarController ammoBar;
	FillBarController iceBar;

	void OnEnable()
	{
		ga = GetComponent<GameActor>();
		hitParticles = GameObject.Find("HitParticles").GetComponent<ParticleSystem>();
		healthBar = GameObject.FindWithTag("HealthBar").GetComponent<FillBarController>();
		ammoBar = GameObject.FindWithTag("AmmoBar").GetComponent<FillBarController>();
		iceBar = GameObject.FindWithTag("IceBar").GetComponent<FillBarController>();
		if(healthBar != null)
			healthBar.max = ga.maxHealth;
		if(ammoBar != null)
			ammoBar.max = maxAmmo;
		if(iceBar != null)
			iceBar.max = freezerGun.maxAmmo;
	}

	void Update()
	{
		ga.moveDir = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

		if(Input.GetButton("Fire")) {
			Fire(Util.MouseToWorldPos(0));
		} else if(freezerGun != null && Input.GetButton("AltFire")) {
			if(freezerGun.CanFire)
				freezerGun.Fire(Util.MouseToWorldPos(0));
			else
				freezerGun.StopFire();
		} else if(freezerGun != null && Input.GetButtonUp("AltFire")) {
			freezerGun.StopFire();
		}

		if(healthBar != null)
			healthBar.value = ga.GetHealth();
		if(ammoBar != null)
			ammoBar.value = ammo;
		if(iceBar != null) {
			if(haveFreezerGun)
				iceBar.value = freezerGun.ammo;
			else
				iceBar.value = 0;
		}
	}

	void Fire(Vector3 target)
	{
		if(canFire && ammo > 0) {
			//Debug.Log ("Firing at " + target);
			RaycastHit2D hit;
			if(hit = Physics2D.Raycast(transform.position,(target-transform.position).normalized,100,shootable.value)) {
				lr.enabled = true;
				Vector3 pos1 = transform.position;
				pos1.z = -1;
				Vector3 pos2 = hit.point;
				pos2.z = -1;
				lr.SetPosition(0,pos1);
				lr.SetPosition(1,pos2);
				hitParticles.transform.position = pos2;
				hitParticles.Play();
				GameActor hitGA;
				if(hitGA = hit.transform.GetComponent<GameActor>()) {
					hitGA.TakeDamage(damage);
				}

			}
			ammo -= 1;
			canFire = false;
			StartCoroutine(Timers.Countdown(1/attackSpeed,() => {canFire = true;}));
			StartCoroutine(Timers.Countdown(fireLineVisibilityTime,() => lr.enabled = false));
		}
	}

	public void ReceiveAmmo(float amount)
	{
		ammo = Mathf.Clamp(ammo + amount,0,maxAmmo);
	}

	public void ReceiveFreezerAmmo(float amount)
	{
		if(freezerGun != null)
			freezerGun.ReceiveAmmo(amount);
	}

	public void Die()
	{
		GetComponent<SpriteRenderer>().enabled = false;
		Time.timeScale = 0;
		deathParticles.Play();
		GameObject.Find ("RoomManager").GetComponent<RoomManager>().paused = true;
		StartCoroutine(Timers.CountdownRealtime(deathParticles.ps.duration,EndGame));
	}

	void EndGame()
	{
		deathParticles.Stop();
		GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
	}


}
