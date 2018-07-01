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

	[Range(0.0f, 1.0f)]
	public float[] PlayerAudioClipsVolume;
	[Range(0.0f, 1.0f)]
	public float[] EnemyAudioClipsVolume;

	private LPGameMode gameModeReference;

	private bool playSuccess = true;
	private bool played = false;

    void Start () 
	{
		LPBaseCharacter.OnCharacterDie += FailEnd;
		LPBaseCharacter.OnCharacterFinishLevel += SuccessEnd;
		LPBaseCharacter.OnCharacterJump += PlayJumpAudio;
		LPBaseCollectable.OnCollectedSpecial += PlayItemCollectAudio;
		LPBaseCollectable.OnCollectedCoin += PlayCoinCollectAudio;
		LPBaseEnemy.OnEnemyGotHit += PlayEnemyHitAudio;
		LPBaseEnemy.OnEnemyHitFloor += PlayEnemyHitFloorAudio;

		gameModeReference = GameObject.FindObjectOfType<LPGameMode>();

		gameModeReference.SoundtrackAudioSource.clip = PlaySoundtrack();
		gameModeReference.SoundtrackAudioSource.Play();
	}
    
	void Update () 
	{
		if (gameModeReference.WillChangeSoundtrack) {

			if (gameModeReference.SoundtrackAudioSource.volume <= 0.1f && !played) {
			
				gameModeReference.SoundtrackAudioSource.Stop();
			
				if (playSuccess)
					gameModeReference.SoundtrackAudioSource.clip = SoundtrackLevelEndSuccess;
				else
					gameModeReference.SoundtrackAudioSource.clip = SoundtrackLevelEndFail;				
			
				played = true;
				gameModeReference.SoundtrackAudioSource.Play();

			} else if (!played) {
				gameModeReference.SoundtrackAudioSource.volume = Mathf.Lerp(gameModeReference.SoundtrackAudioSource.volume, 0.0f, 0.1f);
			} else {
				gameModeReference.SoundtrackAudioSource.volume = Mathf.Lerp(gameModeReference.SoundtrackAudioSource.volume, 1.0f, 0.008f);
			}
		}
	}

	void SuccessEnd()
	{
		playSuccess = true;
		RemoveDelegates();
	}

	void FailEnd()
	{
		playSuccess = false;
		RemoveDelegates();
	}

	void PlayItemCollectAudio(PowerUp powerUp)
	{
		gameModeReference.SoundEffectsAudioSource.volume = PlayerAudioClipsVolume[2];
		gameModeReference.SoundEffectsAudioSource.PlayOneShot(PlayerAudioClips[2]);
	}

	void PlayCoinCollectAudio(int value)
	{
		gameModeReference.SoundEffectsAudioSource.volume = PlayerAudioClipsVolume[1];
		gameModeReference.SoundEffectsAudioSource.PlayOneShot(PlayerAudioClips[1]);
	}

	void PlayJumpAudio()
	{
		gameModeReference.SoundEffectsAudioSource.volume = PlayerAudioClipsVolume[0];
		gameModeReference.SoundEffectsAudioSource.PlayOneShot(PlayerAudioClips[0]);
	}


	void PlayEnemyHitAudio()
	{
		gameModeReference.SoundEffectsAudioSource.volume = EnemyAudioClipsVolume[0];
		gameModeReference.SoundEffectsAudioSource.PlayOneShot(EnemyAudioClips[0]);
	}


	void PlayEnemyHitFloorAudio()
	{
		gameModeReference.SoundEffectsAudioSource.volume = EnemyAudioClipsVolume[1];
		gameModeReference.SoundEffectsAudioSource.PlayOneShot(EnemyAudioClips[1]);
	}

	void RemoveDelegates()
	{
		LPBaseCharacter.OnCharacterDie -= FailEnd;
		LPBaseCharacter.OnCharacterFinishLevel -= SuccessEnd;
		LPBaseCharacter.OnCharacterJump -= PlayJumpAudio;
		LPBaseCollectable.OnCollectedSpecial -= PlayItemCollectAudio;
		LPBaseCollectable.OnCollectedCoin -= PlayCoinCollectAudio;
		LPBaseEnemy.OnEnemyGotHit -= PlayEnemyHitAudio;
		LPBaseEnemy.OnEnemyHitFloor -= PlayEnemyHitFloorAudio;
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
