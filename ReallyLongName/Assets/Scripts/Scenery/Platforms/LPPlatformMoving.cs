using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPPlatformMoving : LPBasePlatform
{
    public bool MoveOnAwake;

    void Start()
    {
        base.Start();

        if(!MoveOnAwake)
            TurnOff();
    }
    
    void Update()
    {
        base.Update();
    }
}