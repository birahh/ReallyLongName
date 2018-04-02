using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemyMelee : LPBaseEnemy
{

    void Start()
    {
		base.Start();

		Speed = 1.5f;
    }


    void Update()
    {
		base.Update();
    }
}