using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemyMelee : LPBaseEnemy
{

    void Start()
    {
		base.Start();

		Speed = 1.5f;
    }


    void Update()
    {
		base.Update();
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Equals("Player")) {
            print("isPlayer");
            if (coll.GetComponentInParent<LPPlayableCharacter>().IsEnemyBelow) {
                print("isEnemyBelow");
                GameObject.Destroy(gameObject);
            }
        }
    }
}