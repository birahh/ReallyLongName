using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPCollectableSpecial : LPBaseCollectable
{
    public PowerUp PowerUp;

    void Start()
    {
        base.Start();
        base.powerUp = PowerUp;
    }
}