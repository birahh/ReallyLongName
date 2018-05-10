using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LPGameInstance
{
    public static LPBaseObject[] SceneObjects;
    public static string CurrentScene = "BaseScene";

    public static int CoinAmount = 0;
    public static int ContinueAmount = 2;

    public static void ReloadCurrentScene()
    {
        SceneManager.LoadScene(CurrentScene);

        Scene scene = SceneManager.GetSceneByName(CurrentScene);
        SceneManager.SetActiveScene(scene);
    }

    public static void LoadMap(string sceneName)
    {
        CurrentScene = sceneName;

        SceneObjects = new LPBaseObject[0];

        SceneManager.LoadScene(sceneName);

        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);

        SceneObjects = Object.FindObjectsOfType<LPBaseObject>() as LPBaseObject[];
    }

    public static void Checkpoint()
    {
        SceneObjects = new LPBaseObject[0];
        SceneObjects = Object.FindObjectsOfType<LPBaseObject>() as LPBaseObject[];
    }
}
