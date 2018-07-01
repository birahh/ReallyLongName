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

    void Start ()
    {
		if (LPGameInstance.GameModeInstance == null) {
			LPGameInstance.SetGameModeInstance (this);
		}

		if (LPGameInstance.GameModeInstance == this) {		
			LPPlayableCharacter.OnCharacterDie += PlayerDiedWithDelay;
			LPPlayableCharacter.OnCharacterFinishLevel += LoadNextLevelWithDelay;	
			LPGameInstance.TransitionScene = TransitionScene;
			LPGameInstance.MenuScene = MenuScene;
			LPGameInstance.GameScenes = GameScenes;
			LPGameInstance.GameOverScene = GameOverScene;
			LPGameInstance.FinalScene = FinalScene;
		} 
	}
	
	void Update()
    {
	}

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
