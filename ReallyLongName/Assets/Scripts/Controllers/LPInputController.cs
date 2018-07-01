using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (LPPlayableCharacter))]
public class LPInputController : MonoBehaviour 
{
	LPPlayableCharacter player;
	bool canJumpAgain = true;
	bool isJumping = false;

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

        #region JumpKeys
        if (directionalInput.y > 0) 
			if (canJumpAgain) {
				isJumping = false;
				canJumpAgain = false;
				player.OnJumpInputDown ();
			}

		if (directionalInput.y <= 0) 
			if (!isJumping) {
				isJumping = true;
				canJumpAgain = true;
				player.OnJumpInputUp ();
			}

		if (Input.GetKeyDown (KeyCode.Space)) {
			player.OnJumpInputDown ();
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			player.OnJumpInputUp ();
        }
        #endregion

        #region RunningKeys
        if (Input.GetKeyDown (KeyCode.LeftShift)) {
			player.IsRunning = true;
		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			player.IsRunning = false;
		}
        #endregion

		#region RestartLevel
		if (Input.GetKeyUp (KeyCode.R)) {
			LPGameInstance.LoadTransitionScene();
		}
		#endregion
    }
}