using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPBaseCharacter : LP2DController
{
    public int Life;
    public bool CanGetHit = true;

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

        CanGetHit = false;
        Invoke("HitCooldownReset", LPDefinitions.Character_HitCooldown);
    }

    void HitCooldownReset()
    {
        CanGetHit = true;
    }

    void Die()
    {
        print("Player Died");
    }
}