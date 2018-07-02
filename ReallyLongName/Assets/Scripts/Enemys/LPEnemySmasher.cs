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
        base.Activate(delayTime);
    }

	void CameraShake()
	{
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