using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LPGameMode : MonoBehaviour
{
    public string TransitionScene;  //  As Default -2 in CurrentScene
    public string MenuScene;        //  As Default -1 in CurrentScene
    public string[] GameScenes;     //  As Default it's index in CurrentScene

    private int currentScene = -1;

    void Start ()
    {
        LPPlayableCharacter.OnCharacterDie += PlayerDied;
	}
	
	
	void Update ()
    {
		
	}

    void PlayerDied ()
    {        
        Invoke("RestartScene", 1.0f);
    }

    void RestartScene ()
    {
        LPGameInstance.ReloadMap();
    }

    public void LoadMenuScene()
    {

    }

    public void LoadMenuScene()
    {

    }
}
