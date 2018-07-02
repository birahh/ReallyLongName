using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LPHUDController : MonoBehaviour 
{
	public Text timer;
	public Text coinCounter;
	public Text continueCounter;

    int coinAmount = 0;
    int continueAmount = 2;

	float initialTime = 300.0f;
	bool isFinished = false;

	LPPlayableCharacter playerReference;


	// Use this for initialization
	void Start () 
	{
        continueAmount = LPGameInstance.ContinueAmount;

		playerReference = GameObject.FindObjectOfType<LPPlayableCharacter>();
	}

	// Update is called once per frame
	void Update () 
	{
		if (!isFinished) {
			
			if (initialTime > 0) {
					
				initialTime = initialTime - Time.deltaTime;

		        coinAmount = LPGameInstance.LevelCoinAmount;
		        continueAmount = LPGameInstance.ContinueAmount;

				UpdateHUDText();

			} else {

				isFinished = true;

				initialTime = 0;
				UpdateHUDText();

				if (playerReference)
					playerReference.Hit();
			}
		}
    }

    void UpdateHUDText()
    {
        timer.text = "" + Mathf.FloorToInt(initialTime);
        coinCounter.text = coinAmount+"";
        continueCounter.text = "x" + continueAmount;
    }
}
