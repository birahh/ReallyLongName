using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemyShooter : LPBaseEnemy
{	
    void Start()
    {
        Speed = LPDefinitions.Shooter_Speed;

        dontComeBack = true;        

        base.Start();
    }


    void Update()
    {
        base.Update();
    }
    
    public void Activate(float delayTime)
    {
        base.Activate(delayTime);
    }
}