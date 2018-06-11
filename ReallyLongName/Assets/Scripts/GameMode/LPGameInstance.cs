using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LPGameInstance
{
	public static LPBaseObject[] SceneObjects;
	public static string[] GameScenes;
	public static string MenuScene;
	public static string TransitionScene;
	public static string GameOverScene;
	public static string FinalScene;

	public static int CurrentLevel = -2;

    public static int TotalCoinAmount = 0;
    public static int LevelCoinAmount = 0;
    public static int ContinueAmount = 2;

	#region Scene Transitions

	public static void PlayerDied()
	{
		LoadTransitionScene();

		if (ContinueAmount > 0)
			ReloadCurrentScene();
		else 
			LoadGameOverScene();
	}

    public static void ReloadCurrentScene()
    {
		LoadScene(CurrentLevel);
    }

	public static void LoadTransitionScene()
	{
		LoadScene(-2);
	}

	public static void LoadGameOverScene()
	{
		LoadScene(-3);
	}

	public static void LoadMenuScene()
	{
		LoadScene(-1);
	}

	public static void LoadFinalScene()
	{
		LoadScene(-4);
	}

	public static void LoadNextScene()
	{
		int nextLevel = CurrentLevel + 1;

		if (GameScenes.Length - 1 <= nextLevel)
			LoadScene(nextLevel);
		else
			LoadFinalScene();
	}

	public static string GetSceneName(int levelId)
	{

		switch (levelId) {
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

	public static void LoadScene (int nextLevel)
    {
		string sceneName = GetSceneName(nextLevel);

        LevelCoinAmount = 0;

        SceneObjects = new LPBaseObject[0];

        SceneManager.LoadScene(sceneName);

        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);

        SceneObjects = Object.FindObjectsOfType<LPBaseObject>() as LPBaseObject[];
	}
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
