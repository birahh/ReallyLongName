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

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag.Equals("Player")) {

            if (enemy.GetType() == typeof(LPEnemySmasher))
                (enemy as LPEnemySmasher).Activate();

            if (enemy.GetType() == typeof(LPEnemyShooter))
                (enemy as LPEnemyShooter).Activate();

            if (enemy.GetType() == typeof(LPEnemySaw))
                (enemy as LPEnemySaw).Activate();

            if (enemy.GetType() == typeof(LPEnemyMelee))
                (enemy as LPEnemyMelee).Activate();
        }	
	}
}
