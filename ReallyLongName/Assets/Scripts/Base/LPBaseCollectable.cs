using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPBaseCollectable : LPBaseObject
{
    public delegate void Collected(int value);
    public static event Collected OnCollected;
    
    protected int value;

    void Start()
    {

    }
    
    void Update()
    {

    }
    
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Equals("Player")) {

            if (OnCollected != null)
                OnCollected(value);

            GameObject.Destroy(gameObject);
        }
    }
}