using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPDeathZone : MonoBehaviour
{
	void Start ()
    {
        GetComponent<MeshRenderer>().material.color = new Vector4(1.0f, 1.0f, 1.0f, 0.01f);		
	}


    void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.tag.Equals("Player"))
            if (coll.GetComponent<LPPlayableCharacter>().IsAlive)
                coll.GetComponent<LPPlayableCharacter>().Die();
    }
}
