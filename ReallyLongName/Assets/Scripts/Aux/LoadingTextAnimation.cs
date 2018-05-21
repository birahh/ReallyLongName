using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTextAnimation : MonoBehaviour 
{
	string[] text;
	string baseText;

	void Start () 
	{
		text = new string[3];

		text[0] = ".";
		text[1] = "..";
		text[2] = "...";

		baseText = GetComponent<Text>().text;
	}
	

	void Update () 
	{
	}
}
