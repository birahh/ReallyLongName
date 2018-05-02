using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemyMelee : LPBaseEnemy
{

    void Start()
    {
		base.Start();

		Speed = LPDefinitions.Melee_Speed;
    }


    void Update()
    {
		base.Update();
    }
}