using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemyMelee : LPBaseEnemy
{
	private float nextAngleMaxLimit = 180.0f;
	private float nextAngleMinLimit = 0.0f;
	private bool nextAngleChanged = true;
	float nextAngle=0;

    void Start()
    {
		base.Start();

		Speed = LPDefinitions.Melee_Speed;
    }


    void Update()
    {
		base.Update();

		if (ShouldRotate) {

			if (animator) {
				animator.SetBool("isWalking", false);
			}
			
			nextAngleChanged = false;

			nextAngle = Mathf.Lerp(nextAngle,nextAngleMaxLimit, 0.2f);

			body.transform.eulerAngles = new Vector3(body.transform.eulerAngles.x, nextAngle, body.transform.eulerAngles.z);
		
		} else {

			animator.SetBool("isWalking", true);
			
			if (!nextAngleChanged) {
				
				if (nextAngleMaxLimit == 180.0f) {
					nextAngleMaxLimit = 360.0f;
					nextAngle = 180.0f;
				} else {
					nextAngleMaxLimit = 180.0f;
					nextAngle = 0.0f;
				}

				nextAngleChanged = true;
			}
		}

    }
}