using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPAudioController : MonoBehaviour
{
    public AudioClip[] SFXMenu;

	public AudioClip[] SoundtrackLevels;
	public AudioClip SoundtrackFinalScene;
	public AudioClip SoundtrackGameOverScene;
	public AudioClip SoundtrackMenuScene;
	public AudioClip SoundtrackLevelEndSuccess;
	public AudioClip SoundtrackLevelEndFail;

	public AudioClip[] PlayerAudioClips;

	public AudioClip[] EnemyAudioClips;

    void Start () 
	{
		
	}
    
	void Update () 
	{
		
	}

	AudioClip PlaySoundtrack()
	{
		switch (LPGameInstance.CurrentLevel) {
		case -4: 
			return SoundtrackFinalScene;
			break; 
		case -3: 
			return SoundtrackGameOverScene;
			break; 
		case -2: 
			return null;
			break; 
		case -1:
			return SoundtrackMenuScene;
			break;
		default:
			return SoundtrackLevels[LPGameInstance.CurrentLevel];
			break;
		}
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
