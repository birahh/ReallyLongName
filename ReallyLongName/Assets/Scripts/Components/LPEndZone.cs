using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEndZone : MonoBehaviour 
{
	
	void Start () 
	{
		
	}
	
    void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.tag.Equals("Player"))
            if (coll.GetComponent<LPPlayableCharacter>().IsAlive)
                coll.GetComponent<LPPlayableCharacter>().FinishLevel();
    }
}
