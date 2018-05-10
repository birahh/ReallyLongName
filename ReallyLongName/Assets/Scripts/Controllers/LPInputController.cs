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

		float motionX = 0.0f;

		motionX = (Input.GetAxisRaw("Horizontal")>LPDefinitions.Character_MotionThreshold)?1:(Input.GetAxisRaw("Horizontal")<-LPDefinitions.Character_MotionThreshold) ?-1:motionX;

		player.SetDirectionalInput (directionalInput, motionX);

		if (Input.GetKeyDown (KeyCode.Space) || directionalInput.y > 0.0f) {
			player.OnJumpInputDown ();
		}

		if (Input.GetKeyUp (KeyCode.Space) || directionalInput.y <= 0.0f) {
			player.OnJumpInputUp ();
		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			player.IsRunning = true;
		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			player.IsRunning = false;
		}
	}
}