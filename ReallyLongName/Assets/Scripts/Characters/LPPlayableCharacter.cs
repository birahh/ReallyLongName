using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPPlayableCharacter : LPBaseCharacter
{
	float maxJumpHeight = 4;
	float minJumpHeight = 1;
	float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;
	public bool isRunning;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	float velocityXSmoothing;
	public float groundSlideFactor = 1f;
	Vector3 velocity;

	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;

	void Start() 
	{
		base.Start();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);

		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	void Update() 
	{
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2) + Mathf.Clamp(LPDefinitions.World_Gravity, 1, 100);	//	WorldGravity Attenuation

		maxJumpHeight = Mathf.Clamp(LPDefinitions.Character_MaxJumpHeight, 4, 10);
		minJumpHeight = Mathf.Clamp(LPDefinitions.Character_MinJumpHeight, 1, 5);

		wallSlideSpeedMax = Mathf.Clamp(LPDefinitions.Character_WallSlideSpeed, 1, 10);
		wallStickTime = Mathf.Clamp(LPDefinitions.Character_WallStickTime, 0, 5);

		if (isRunning)
			moveSpeed = Mathf.Clamp(LPDefinitions.Character_Speed, 1, 20) * 2;
		else 
			moveSpeed = Mathf.Clamp(LPDefinitions.Character_Speed, 1, 20);

		CalculateVelocity ();
		HandleWallSliding ();

		Move (velocity * Time.deltaTime, directionalInput);

		if (collisions.above || collisions.below) {
			if (collisions.slidingDownMaxSlope) {
				velocity.y += collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}
	}

	public void SetDirectionalInput (Vector2 input) 
	{
		directionalInput = input;
	}

	public void OnJumpInputDown() 
	{
		if (wallSliding) {
			if (wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (collisions.below) {
			if (collisions.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign (collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() 
	{
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

	void HandleWallSliding() 
	{
		wallDirX = (collisions.left) ? -1 : 1;
		wallSliding = false;

		if ((collisions.left || collisions.right) && !collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				} else {
					timeToWallUnstick = wallStickTime;
				}
			} else {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

	void CalculateVelocity() 
	{
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.Lerp( velocity.x, 
								 Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne), 
								 groundSlideFactor );
		velocity.y += gravity * Time.deltaTime;
	}
}