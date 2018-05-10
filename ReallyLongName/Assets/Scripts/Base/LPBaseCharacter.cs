using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPBaseCharacter : LP2DController
{
    public int Life;
    public bool CanGetHit = true;

    public delegate void CharacterDie();
    public static event CharacterDie OnCharacterDie;

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

    public void Die()
    {
        if (OnCharacterDie != null) {
            OnCharacterDie();
            CanGetHit = false;
        }        
    }
}