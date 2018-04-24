using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemySmasher : LPBaseEnemy
{
    public float WaitTimeBeforeFall;

    void Start()
    {
		base.Start();
    }

    void Update()
    {
		base.Update();        
    }

    public void Activate()
    {
        Invoke("CallBaseActivate", WaitTimeBeforeFall);
    }

    void CallBaseActivate()
    {
        base.Activate(1);
    }
}