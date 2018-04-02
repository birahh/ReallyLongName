using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (LPPlayableCharacter))]
public class LPInputController : MonoBehaviour 
{
	LPPlayableCharacter player;

	void Start ()
	{
		player = GetComponent<LPPlayableCharacter>();
	}

	void Update ()
	{
		Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		player.SetDirectionalInput (directionalInput);

		if (Input.GetKeyDown (KeyCode.Space)) {
			player.OnJumpInputDown ();
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			player.OnJumpInputUp ();
		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			player.isRunning = true;
		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			player.isRunning = false;
		}
	}
}