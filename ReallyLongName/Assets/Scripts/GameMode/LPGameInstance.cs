using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LPGameInstance
{
    public static LPBaseObject[] SceneObjects;
    public static string CurrentScene = "BaseScene";

    public static int TotalCoinAmount = 0;
    public static int LevelCoinAmount = 0;
    public static int ContinueAmount = 2;

    public static void ReloadCurrentScene ()
    {
        SceneManager.LoadScene(CurrentScene);

        Scene scene = SceneManager.GetSceneByName(CurrentScene);
        SceneManager.SetActiveScene(scene);
    }

    public static void ReloadMap()
    {
        LoadMap(CurrentScene);
    }

    public static void LoadMap (string sceneName)
    {
        LevelCoinAmount = 0;

        CurrentScene = sceneName;

        SceneObjects = new LPBaseObject[0];

        SceneManager.LoadScene(sceneName);

        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);

        SceneObjects = Object.FindObjectsOfType<LPBaseObject>() as LPBaseObject[];
    }

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
}
