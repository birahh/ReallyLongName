using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPBaseCollectable : LPBaseObject
{
	public ParticleSystem CollectableParticle;

    public delegate void CollectedCoin(int value);
    public delegate void CollectedSpecial(PowerUp powerUp);
    public static event CollectedCoin OnCollectedCoin;
    public static event CollectedSpecial OnCollectedSpecial;

    protected int value;
    protected PowerUp powerUp;
    protected bool shouldFollow = false;
    protected Transform targetToFollow;

    float followFactor = LPDefinitions.Coin_FollowFactor;

    void Start()
    {

    }
    
    void Update()
    {
        if (shouldFollow) {

            float newX = Mathf.Lerp(transform.position.x, targetToFollow.position.x, followFactor * 2f * Speed);
            float newY = Mathf.Lerp(transform.position.y, targetToFollow.position.y, followFactor * 1.5f * Speed);
            float newZ = transform.position.z;
     
            transform.position = new Vector3(newX, newY, newZ);

            if (transform.position == targetToFollow.position)
                shouldFollow = false;
        }
    }
    
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Equals("CoinCatcher")) {

            LPPlayableCharacter player = coll.transform.parent.GetComponent<LPPlayableCharacter>();
            
            if (player.CurrentPowerUp == PowerUp.Magnet) {

				TurnOffCollider();
                targetToFollow = player.transform;
                shouldFollow = true;
            }
        }

        if (coll.tag.Equals("Player")) {

			TurnOffCollider();
			TurnOffRenderer();
            shouldFollow = false;
            SelfDestroy();
        }
    }

    public void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag.Equals("CoinCatcher"))
        {
            LPPlayableCharacter player = coll.transform.parent.GetComponent<LPPlayableCharacter>();

            if (player.CurrentPowerUp == PowerUp.Magnet)
            {
                targetToFollow = player.transform;
                shouldFollow = true;
            }
        }

        if (coll.tag.Equals("Player")) {
            shouldFollow = false;
            SelfDestroy();
        }
    }

	void TurnOffCollider()
	{
		GetComponent<Collider2D>().enabled = false;
	}

	void TurnOffRenderer()
	{
		GetComponent<Renderer>().enabled = false;
	}

    void SelfDestroy()
    {
		CollectableParticle.Play();
		
        if(OnCollectedCoin != null)
            if(this.GetType() == typeof(LPCollectableCoin))
                OnCollectedCoin(value);

        if (OnCollectedSpecial != null)
            if (this.GetType() == typeof(LPCollectableSpecial))
                OnCollectedSpecial(powerUp);

		Invoke("SelfDestroyWithDelay", 1.5f);
    }

	void SelfDestroyWithDelay()
	{
		GameObject.Destroy(gameObject);
	}
}