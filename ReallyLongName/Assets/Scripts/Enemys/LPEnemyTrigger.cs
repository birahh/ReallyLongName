using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemyTrigger : MonoBehaviour 
{
	public LPBaseEnemy enemy;
	
	void Start () 
	{
		
	}

	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag.Equals("Player")) {
			enemy.Activate();
		}	
	}
}
