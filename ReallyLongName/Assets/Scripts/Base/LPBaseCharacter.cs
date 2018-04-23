using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPBaseCharacter : LP2DController
{
    public int Life;

    void Start()
    {
		base.Start();
    }

    public void Update()
    {
        base.Update();
    }

    public void Hit()
    {
        Life--;

        if (Life <=  0) {
            Die();
        }

        print("HIT");
    }

    void Die()
    {
        print("Player Died");
    }
}