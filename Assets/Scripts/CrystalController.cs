using UnityEngine;
using System.Collections;

public class CrystalController : MonoBehaviour
{

	public Room room;
	public float chargeDuration;
	float chargeTime = 0;
	public bool activated = false;
	public BorderAnim anim;
	bool charging = false;
	bool destroying = false;
	float destroyStartTime;


	void Update()
	{
		if(!activated && !charging)
			chargeTime = Mathf.Clamp(0,chargeTime - Time.deltaTime,chargeDuration);
		else if(destroying) 
			chargeTime = Mathf.PingPong(2*(Time.time-destroyStartTime+1),1);
		
		anim.fill = chargeTime/chargeDuration;
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if(!activated && coll.gameObject.layer == LayerMask.NameToLayer("Player")) {
			chargeTime += Time.deltaTime;
			charging = true;

			if(chargeTime >= chargeDuration) {
				activated = true;
				room.Activate();
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if(!activated)
			charging = false;
	}

	public void Deactivate()
	{
		activated = false;
		charging = false;
		chargeTime = 0;
	}

	public void StartDestroy()
	{
		destroyStartTime = Time.time;
		destroying = true;
	}

}

