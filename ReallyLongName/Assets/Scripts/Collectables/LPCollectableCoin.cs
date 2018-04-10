using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPCollectableCoin : LPBaseCollectable
{
    public int CoinValue;

    void Start()
    {
        base.Start();
        base.value = CoinValue;
    }
}