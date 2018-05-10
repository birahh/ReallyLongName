using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LPGameMode : MonoBehaviour
{


	void Start ()
    {
        LPPlayableCharacter.OnCharacterDie += PlayerDied;
	}
	
	
	void Update ()
    {
		
	}

    void PlayerDied ()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Invoke("RestartScene", 1.0f);
    }

    void RestartScene ()
    {
        SceneManager.LoadScene(LPGameInstance.CurrentScene, LoadSceneMode.Single);
    }
}
