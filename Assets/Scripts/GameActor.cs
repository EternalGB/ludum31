using UnityEngine;
using System.Collections;

public class GameActor : MonoBehaviour
{

	public float speed;
	float actualSpeed;
	float health;
	public float maxHealth;
	public bool canFreeze;
	public float maxFreeze;
	public float frozenSolidPercentage;
	public float thawRate;
	float freeze;
	public bool beingFrozen = false;
	public Vector2 moveDir;

	void OnEnable()
	{
		health = maxHealth;
		freeze = 0;
	}

	void Update()
	{
		if(canFreeze) {
			if(freeze >= maxFreeze*frozenSolidPercentage) {
				DisableAllScripts();
				moveDir = Vector2.zero;
				//TODO frozen display
			} else {
				EnableAllScripts();
			}
			actualSpeed = Mathf.Lerp(speed,0,freeze/maxFreeze);
		} else {
			actualSpeed = speed;
		}
		   	


		rigidbody2D.velocity = Vector2.ClampMagnitude(moveDir*actualSpeed,actualSpeed);

		if(health <= 0)
			SendMessage("Die");

		freeze = Mathf.Clamp(freeze - thawRate*Time.deltaTime,0,maxFreeze);
	}

	public void TakeDamage(float amount)
	{
		health -= amount;
	}

	public void Heal(float amount)
	{
		health = Mathf.Clamp(health+amount,0,100);
	}

	public void TakeCold(float amount)
	{
		if(canFreeze) {
			freeze = Mathf.Clamp (freeze + amount,0,maxFreeze);
		}
	}

	public float GetHealth()
	{
		return health;
	}

	void DisableAllScripts()
	{
		MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
		foreach(MonoBehaviour mb in behaviours) {
			if(!mb.Equals(this))
				mb.enabled = false;
		}
	}

	void EnableAllScripts()
	{
		MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
		foreach(MonoBehaviour mb in behaviours) {
			if(!mb.Equals(this))
				mb.enabled = true;
		}
	}

}

