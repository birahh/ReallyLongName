using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemyTrigger : MonoBehaviour 
{
	public LPBaseEnemy enemy;
    public bool ShouldActivate = true;

    private bool isActive = true;
	
	void Start () 
	{
        
	}

	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag.Equals("Player") && isActive) {

            isActive = false;


            if (enemy)
            {
                //  Reset Enemies
                if (!ShouldActivate) {

                    if (enemy.GetType() == typeof(LPEnemySmasher))
                        (enemy as LPEnemySmasher).Reset();

                    if (enemy.GetType() == typeof(LPEnemyShooter))
                        (enemy as LPEnemyShooter).Reset();

                    if (enemy.GetType() == typeof(LPEnemySaw))
                        (enemy as LPEnemySaw).Reset();

                    if (enemy.GetType() == typeof(LPEnemyMelee))
                        (enemy as LPEnemyMelee).Reset();

                //  Activate Enemies
                } else {

                    if (enemy.GetType() == typeof(LPEnemySmasher))
                        (enemy as LPEnemySmasher).Activate(LPDefinitions.Smasher_BeforeFallTime);

                    if (enemy.GetType() == typeof(LPEnemyShooter))
                        (enemy as LPEnemyShooter).Activate(LPDefinitions.Shooter_BeforeShootTime);

                    if (enemy.GetType() == typeof(LPEnemySaw))
                        (enemy as LPEnemySaw).Activate();

                    if (enemy.GetType() == typeof(LPEnemyMelee))
                        (enemy as LPEnemyMelee).Activate();
                }

            }
        }	
	}

    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.tag.Equals("Player"))
            isActive = true;
    }
}
