using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LPHUDController : MonoBehaviour 
{
	public Text timer;
	public Text coinCounter;
	public Text continueCounter;

	public float initialTime = 300.0f;

	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{
		initialTime = initialTime - Time.deltaTime;

		timer.text = ""+Mathf.FloorToInt(initialTime);
	}

	public void setCoinCounter (int newValue)
	{
		coinCounter.text = newValue+" Moedas";
	}

	public void setContinueCounter (int newValue)
	{
		continueCounter.text = "x"+newValue;
	}
}
