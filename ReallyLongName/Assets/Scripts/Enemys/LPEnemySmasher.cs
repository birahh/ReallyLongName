using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemySmasher : LPBaseEnemy
{
    public float WaitTimeBeforeFall;

    void Start()
    {
		base.Start();

        base.Activate(1);
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