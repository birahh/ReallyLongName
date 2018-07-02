using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPPlatformGlitch : LPBasePlatform
{
	public ParticleSystem ParticlesIdle;
	public ParticleSystem ParticlesOnTop;
	public ParticleSystem ParticlesFade;

    void Start()
    {
        base.Start();

		TurnOn();
		ParticlesFade.Stop();
		ParticlesFade.Clear();
		ParticlesFade.GetComponent<Renderer>().enabled = false;
		ParticlesOnTop.Stop();
		ParticlesOnTop.Clear();
		ParticlesOnTop.GetComponent<Renderer>().enabled = false;
		ParticlesIdle.GetComponent<Renderer>().enabled = true;
		ParticlesIdle.Play();
    }


    void Update()
    {
        base.Update();

		if (hasPlayerUp && !ParticlesOnTop.isPlaying && isOn) {

			ParticlesFade.Stop();
			ParticlesFade.Clear();
			ParticlesFade.GetComponent<Renderer>().enabled = false;

			ParticlesIdle.Stop();
			ParticlesIdle.Clear();
			ParticlesIdle.GetComponent<Renderer>().enabled = false;

			ParticlesOnTop.GetComponent<Renderer>().enabled = true;
			ParticlesOnTop.Play();

		} else if (!isOn && !ParticlesFade.isPlaying) {

			ParticlesIdle.Stop();
			ParticlesIdle.Clear();
			ParticlesIdle.GetComponent<Renderer>().enabled = false;

			ParticlesOnTop.Stop();
			ParticlesOnTop.Clear();
			ParticlesOnTop.GetComponent<Renderer>().enabled = false;

			ParticlesFade.GetComponent<Renderer>().enabled = true;
			ParticlesFade.Play();

		} else if (!hasPlayerUp && isOn && !ParticlesIdle.isPlaying) {

			ParticlesFade.Stop();
			ParticlesFade.Clear();
			ParticlesFade.GetComponent<Renderer>().enabled = false;

			ParticlesOnTop.Stop();
			ParticlesOnTop.Clear();
			ParticlesOnTop.GetComponent<Renderer>().enabled = false;

			ParticlesIdle.GetComponent<Renderer>().enabled = true;
			ParticlesIdle.Play();
		}
    }
}