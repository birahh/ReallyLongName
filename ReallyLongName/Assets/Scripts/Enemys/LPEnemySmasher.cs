using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPEnemySmasher : LPBaseEnemy
{
    public float WaitTimeBeforeFall;

	protected LPCameraFollow cameraFollowReference;

    void Start()
    {
		base.Start();
		base.Activate(1);

		LPBaseEnemy.OnEnemyHitFloor += CameraShake;

		cameraFollowReference = GameObject.FindObjectOfType<LPCameraFollow>();
    }

    void Update()
    {
		base.Update();        
    }

    public void Activate(float delayTime)
	{
		if (animator) {
			animator.SetBool("Felt", false);
			animator.SetBool("WillFall", true);
		}
        base.Activate(delayTime);
    }

	void CameraShake()
	{
		if (animator) {
			animator.SetBool("WillFall", false);
			animator.SetBool("Felt", true);
		}
		cameraFollowReference.CameraShake = true;

		Invoke("BackToNormal", 0.2f);
	}

	void BackToNormal()
	{
		cameraFollowReference.CameraShake = false;	
	}

	void OnDestroy()
	{
		LPBaseEnemy.OnEnemyHitFloor -= CameraShake;
	}
}