using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPSpikeTrigger : MonoBehaviour
{
    public bool HasColliderWithPlayer = false;
    
	void Start ()
    {
	}
		
	void Update ()
    {
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player")) {
            HasColliderWithPlayer = true;

            LPPlayableCharacter player = collision.gameObject.GetComponent<LPPlayableCharacter>();

            player.AddImpulseUp();
            player.Hit();
            player.AddImpulseBack();
        }
    }
}
