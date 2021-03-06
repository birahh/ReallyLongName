﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LPGameMode : MonoBehaviour
{
	public string FinalScene;  	//  As Default -4 in CurrentScene
	public string GameOverScene;  	//  As Default -3 in CurrentScene
	public string TransitionScene;  //  As Default -2 in CurrentScene
    public string MenuScene;        //  As Default -1 in CurrentScene
    public string[] GameScenes;     //  As Default it's index in CurrentScene

	public AudioSource SoundtrackAudioSource;
	public AudioSource SoundEffectsAudioSource;

	public bool WillChangeSoundtrack = false;

    public GameObject PauseScreen;

    private float initialVolumeSoundtrack;
    private float initialVolumeSoundEffects;

    void Start ()
    {
		if (LPGameInstance.GameModeInstance == null) {
			LPGameInstance.SetGameModeInstance (this);
        }

        if (LPGameInstance.IsEffectsOn)
            TurnEffectsOn();
        else
            TurnEffectsOff();
        
        if (LPGameInstance.IsMusicOn)
            TurnMusicOn();
        else
            TurnMusicOff();
        
        if (LPGameInstance.GameModeInstance == this) {		
			LPPlayableCharacter.OnCharacterDie += PlayerDiedWithDelay;
			LPPlayableCharacter.OnCharacterFinishLevel += LoadNextLevelWithDelay;
            LPBaseCollectable.OnCollectedSpecial += LPGameInstance.AddContinue;
            LPBaseCollectable.OnCollectedCoin += LPGameInstance.AddCoin;
            LPBaseCharacter.OnCharacterDie += LPGameInstance.RemoveContinue;

            LPGameInstance.TransitionScene = TransitionScene;
			LPGameInstance.MenuScene = MenuScene;
			LPGameInstance.GameScenes = GameScenes;
			LPGameInstance.GameOverScene = GameOverScene;
			LPGameInstance.FinalScene = FinalScene;

            initialVolumeSoundtrack = SoundtrackAudioSource.volume;
            initialVolumeSoundEffects = SoundEffectsAudioSource.volume;
        } 

		if (LPGameInstance.CurrentLevel == -3)
			SoundtrackAudioSource.loop = false;
	}
	
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }

        if (LPGameInstance.IsPaused) {
            SoundtrackAudioSource.volume = initialVolumeSoundtrack * 0.3f;
            SoundEffectsAudioSource.volume = initialVolumeSoundEffects * 0.3f;
        } else {
            SoundtrackAudioSource.volume = initialVolumeSoundtrack;
            SoundEffectsAudioSource.volume = initialVolumeSoundEffects;
        }
    }

    public void TogglePause()
    {
        if (LPGameInstance.IsPaused) {
            PauseScreen.SetActive(LPGameInstance.IsPaused = false);
        } else {
            PauseScreen.SetActive(LPGameInstance.IsPaused = true);
        }
    }

    #region Menu Methods
    public void TurnMusicOff()
    {
        LPGameInstance.IsMusicOn = false;
        SoundtrackAudioSource.mute = true;
    }

    public void TurnMusicOn()
    {
        LPGameInstance.IsMusicOn = true;
        SoundtrackAudioSource.mute = false;
    }

    public void TurnEffectsOff()
    {
        LPGameInstance.IsEffectsOn = false;
        SoundEffectsAudioSource.mute = true;
    }

    public void TurnEffectsOn()
    {
        LPGameInstance.IsEffectsOn = true;
        SoundEffectsAudioSource.mute = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        LPGameInstance.LoadMenuScene();
    }
    #endregion

    public void PlayerDiedWithDelay()
	{
		RemoveDelegates();
		WillChangeSoundtrack = true;
		Invoke("PlayerDied", LPDefinitions.GameMode_TransitionDelay * 0.6f);
    }

    public void PlayerDied()
    {        
		LPGameInstance.PlayerDied();
    }

	public void LoadNextLevelWithDelay() 
	{
		RemoveDelegates();
		WillChangeSoundtrack = true;
		Invoke("LoadNextLevel", LPDefinitions.GameMode_TransitionDelay);
	}

	void RemoveDelegates()
	{
		LPPlayableCharacter.OnCharacterDie -= PlayerDiedWithDelay;
		LPPlayableCharacter.OnCharacterFinishLevel -= LoadNextLevelWithDelay;
        LPBaseCollectable.OnCollectedSpecial -= LPGameInstance.AddContinue;
        LPBaseCollectable.OnCollectedCoin -= LPGameInstance.AddCoin;
        LPBaseCharacter.OnCharacterDie -= LPGameInstance.RemoveContinue;
    }

	void OnDestroy()
	{
		RemoveDelegates();
	}

	public void LoadNextLevel() 
	{
		LPGameInstance.LoadNextScene();
	}

	public void LoadAfterTransition() 
	{
		LPGameInstance.LoadSceneAfterTransition();
	}
}
