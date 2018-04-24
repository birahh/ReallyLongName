using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPBaseCollectable : LPBaseObject
{
    public delegate void Collected(int value);
    public static event Collected OnCollected;
    
    protected int value;
    protected bool shouldFollow = false;
    protected Transform targetToFollow;

    float followFactor = LPDefinitions.Coin_FollowFactor;

    void Start()
    {

    }
    
    void Update()
    {
        if (shouldFollow) {

            float newX = Mathf.Lerp(transform.position.x, targetToFollow.position.x, followFactor);
            float newY = Mathf.Lerp(transform.position.y, targetToFollow.position.y, followFactor);
            float newZ = transform.position.z;

            transform.position = new Vector3(newX, newY, newZ);

            if (transform.position == targetToFollow.position) {

                shouldFollow = false;
            }
        }
    }
    
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Equals("CoinCatcher")) {

            LPPlayableCharacter player = coll.transform.parent.GetComponent<LPPlayableCharacter>();

            if (player.CurrentPowerUp == PowerUp.Magnet) {

                targetToFollow = player.transform;
                shouldFollow = true;
            }
        }

        if (coll.tag.Equals("Player")) {

            shouldFollow = false;
            SelfDestroy();
        }
    }

    void SelfDestroy()
    {
        if (OnCollected != null)
            OnCollected(value);

        GameObject.Destroy(gameObject);
    }
}