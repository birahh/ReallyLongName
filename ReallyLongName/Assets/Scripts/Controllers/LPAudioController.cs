using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPAudioController : MonoBehaviour
{
    public AudioSource[] SFXMenu;

    public AudioSource[] Soundtrack;
    public AudioSource[] Soundscape;

	public AudioSource[] PlayerAudioClips;
	public string[] PlayerAudioNames;

	public AudioSource[] EnemyMelee;
	public string[] EnemyMeleeAudioNames;
    public AudioSource[] EnemySaw;
	public string[] EnemySawAudioNames;
    public AudioSource[] EnemySmasher;
	public string[] EnemySmasherAudioNames;

    void Start () 
	{
		
	}
    
	void Update () 
	{
		
	}

	void PlayEnemyJump(string audioName)
	{
		if (audioName.Equals("Jump")) {
			//EnemyJump[0] as Play
		}

		if (audioName.Equals("Shoot")) {
			//EnemyJump[1] as Play
		}
	}

}
