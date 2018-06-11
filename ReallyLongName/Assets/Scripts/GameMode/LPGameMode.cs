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

    void Start ()
    {
        LPPlayableCharacter.OnCharacterDie += PlayerDied;

		LPGameInstance.TransitionScene = TransitionScene;
		LPGameInstance.MenuScene = MenuScene;
		LPGameInstance.GameScenes = GameScenes;
		LPGameInstance.GameOverScene = GameOverScene;
		LPGameInstance.FinalScene = FinalScene;
	}
	
	
	void Update ()
    {
		
	}

    void PlayerDied ()
    {        
		LPGameInstance.PlayerDied();
    }
}
