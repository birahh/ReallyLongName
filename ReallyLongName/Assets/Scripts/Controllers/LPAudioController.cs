using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPAudioController : MonoBehaviour
{
    public AudioSource[] SFXMenu;

    public AudioSource[] Soundtrack;
    public AudioSource[] Soundscape;

	public AudioSource[] EnemyJumpAudioClips;
	public string[] EnemyJumpAudioNames;
	public AudioSource[] EnemyMelee;
    public AudioSource[] EnemySaw;
    public AudioSource[] EnemySmasher;
    public AudioSource[] EnemyShoot;

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
