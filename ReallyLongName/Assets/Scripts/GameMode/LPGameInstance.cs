using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LPGameInstance
{
	public static LPGameMode GameModeInstance;
	public static LPBaseObject[] SceneObjects;
	public static string[] GameScenes;
	public static string MenuScene;
	public static string TransitionScene;
	public static string GameOverScene;
	public static string FinalScene;

	public static int CurrentLevel = 0;
	public static int NextLevel = 0;

    public static int TotalCoinAmount = 0;
    public static int LevelCoinAmount = 0;
    public static int ContinueAmount = 5;

	#region Game Mode
	public static void SetGameModeInstance (LPGameMode newInstance)
	{
		GameModeInstance = newInstance;
	}
	#endregion

	#region Scene Transitions
	public static void PlayerDied()
	{
		if (ContinueAmount > 0)
			ReloadCurrentScene();
		else 
			LoadGameOverScene();
		
		LoadTransitionScene();
	}

	public static string GetSceneName(int levelId)
	{
		switch (levelId) {
		case -4: 
			return FinalScene;
			break; 
		case -3: 
			return GameOverScene;
			break; 
		case -2: 
			return TransitionScene;
			break; 
		case -1:
			return MenuScene;
			break;
		default:
			return GameScenes[levelId];
			break;
		}
	}	

    public static void ReloadCurrentScene()
    {		
		LoadTransitionScene ();
    }

	public static void LoadTransitionScene()
	{
		LevelCoinAmount = 0;

		SceneObjects = new LPBaseObject[0];

		SceneManager.LoadScene(GetSceneName(-2));
	}

	public static void LoadSceneAfterTransition()
	{
		CurrentLevel = NextLevel;

		if (CurrentLevel > GameScenes.Length - 1) {
			LoadFinalScene();
		} else {
			SceneManager.LoadScene(GetSceneName(CurrentLevel));
		}
	}

	public static void LoadGameOverScene()
	{
		NextLevel = -3;

		LoadTransitionScene();
	}

	public static void LoadMenuScene()
	{
		NextLevel = -1;

		LoadTransitionScene();
	}

	public static void LoadFinalScene()
	{
		NextLevel = -4;

		LoadTransitionScene();
	}

	public static void LoadNextScene()
	{
		NextLevel = CurrentLevel + 1;

		// if (NextLevel > GameScenes.Length - 1) {
			// NextLevel = GameScenes.Length - 1;
		// 	LoadFinalScene();
		// } else {
			LoadTransitionScene();			
		// }
	}

	// public static void LoadScene ()
    // {	
        // LevelCoinAmount = 0;

		// CurrentLevel = NextLevel;

        // SceneObjects = new LPBaseObject[0];

        // SceneManager.LoadScene(GetSceneName(NextLevel));

        // SceneObjects = Object.FindObjectsOfType<LPBaseObject>() as LPBaseObject[];
	// }
	#endregion

	#region Coins and Checkpoints
    public static void Checkpoint ()
    {
        SceneObjects = new LPBaseObject[0];
        SceneObjects = Object.FindObjectsOfType<LPBaseObject>() as LPBaseObject[];
    }

    public static void RemoveContinue()
    {        
        ContinueAmount--;
    }

    public static void AddCoin(int coinAmount)
    {
        TotalCoinAmount += LevelCoinAmount += coinAmount;
    }

    public static void AddContinue(PowerUp powerUp)
    {
        if (powerUp.Equals(PowerUp.Continue))
            ContinueAmount++;
    }

    public static void UpdateData ()
    {

    }
	#endregion
}
