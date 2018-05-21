using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPScreenController : MonoBehaviour 
{
	public GameObject[] screens;
	public string[] screenNames;
		
	void Start () 
	{
		
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.G)) {
			ShowScreen("Coin");
			HideScreen("Continue");
		}

		if (Input.GetKeyDown(KeyCode.H)) {
			ShowScreen("Continue");
			HideScreen("Coin");
		}
	}

	public void ShowScreen (string screenName)
	{
		screens[screenName.IndexOf(screenName)].SetActive(true);
	}
		
	public void HideScreen (string screenName)
	{
		screens[screenName.IndexOf(screenName)].SetActive(false);
	}
}
