﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPGameModeTransition : LPGameMode 
{
	public float TransitionToNextLevelDelay = LPDefinitions.GameMode_TransitionDelay;

	void Start () 
	{
		Invoke("LoadAfterTransition", TransitionToNextLevelDelay);
	}
	
	void Update () {
		
	}
}
