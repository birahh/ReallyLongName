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

	// Use this for initialization
	void Start () 
	{
        LPBaseCollectable.OnCollectedSpecial += LPGameInstance.AddContinue;
        LPBaseCollectable.OnCollectedCoin += LPGameInstance.AddCoin;
        LPBaseCharacter.OnCharacterDie += LPGameInstance.RemoveContinue;

        continueAmount = LPGameInstance.ContinueAmount;
	}

	// Update is called once per frame
	void Update () 
	{
		initialTime = initialTime - Time.deltaTime;

        coinAmount = LPGameInstance.LevelCoinAmount;
        continueAmount = LPGameInstance.ContinueAmount;

        UpdateHUDText();
    }

    void UpdateHUDText()
    {
        timer.text = "" + Mathf.FloorToInt(initialTime);
        coinCounter.text = coinAmount+"";
        continueCounter.text = "x" + continueAmount;
    }
}
