using System.Collections;
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

	private bool created = false;

	public static LPGameMode instance;

//	void Awake()
//	{
//		LPGameMode[] controllers = (LPGameMode[])GameObject.FindObjectsOfType(typeof(LPGameMode));
//
//		if (!created) {
//
//			DontDestroyOnLoad(this.gameObject);
//
//			if (controllers.Length <= 1)
//				created = true;
//		} else 
//			Destroy(gameObject);
//	}
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
//		
//		if (instance == null)
//			instance = this;
//		else if (instance != this)
//			Destroy(gameObject);  
	}

    void Start ()
    {
        LPPlayableCharacter.OnCharacterDie += PlayerDied;

		LPGameInstance.TransitionScene = TransitionScene;
		LPGameInstance.MenuScene = MenuScene;
		LPGameInstance.GameScenes = GameScenes;
		LPGameInstance.GameOverScene = GameOverScene;
		LPGameInstance.FinalScene = FinalScene;

		LPGameInstance.SetGameModeInstance(this);
	}
	
	
	void Update ()
    {
		print("jfdskjfkjsdkfkldsjfklsj");	
	}

	void OnDestroy()
	{
		
	}

    void PlayerDied ()
    {        
		LPGameInstance.PlayerDied();

		if (LPGameInstance.ContinueAmount > 0)
			Invoke("ReloadScene", 1.0f);
		else 
			Invoke("LoadGameOverScene", 1.0f);
	}

	public void ReloadScene()
	{
		LPGameInstance.ReloadCurrentScene();
	}

	public void LoadGameOverScene()
	{
		LPGameInstance.LoadGameOverScene();
	}
}
