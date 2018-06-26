using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPRollPin : LPBaseRollPin
{
    private Rigidbody2D rigidbody;
    public float Torque;
    public float xDiff;

    private float partialTorque;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ChangeTorque(Mathf.Lerp(partialTorque, Torque, 0.8f));
    }

    public void OnTriggerStay2D (Collider2D coll)
    {
        if (coll.tag.Equals("Player")) {
            
            Torque = ( transform.position.x - coll.transform.position.x );
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.Equals("Player")) {

            Torque = 0.0f;
        }
    }

    void ChangeTorque(float newTorque)
    {
        partialTorque = newTorque;
        rigidbody.AddTorque(partialTorque);
    }
}