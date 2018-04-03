using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemySmasher : LPBaseEnemy
{

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
        Invoke("CallBaseActivate", 1.5f);
    }

    void CallBaseActivate()
    {
        base.Activate(1);
    }
}