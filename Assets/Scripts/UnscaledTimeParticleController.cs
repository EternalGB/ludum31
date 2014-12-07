using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class UnscaledTimeParticleController : MonoBehaviour
{

	public ParticleSystem ps;
	float time;
	bool playing = false;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	public void Play()
	{
		playing = true;
		time = 0;
	}

	void Update()
	{
		if(playing) {
			time += Time.unscaledDeltaTime;
			ps.Simulate(time);
		}
	}

	public void Stop()
	{
		playing = false;
	}


}

