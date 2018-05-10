using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPPlayableCharacter : LPBaseCharacter
{
    #region GroundMovementation
    public bool IsRunning;
    public bool IsFalling;
	float moveSpeed = 6f;
    float velocityXSmoothing;
    float groundSlideFactor = LPDefinitions.Character_GroundSlideFactor;
    Vector3 velocity;
    #endregion

    #region JumpMovementation
    int maxJumpCount = LPDefinitions.Character_MaxJumpCount;
    int jumpCount;

    float maxJumpHeight = 3.0f;
    float minJumpHeight = 0.5f;
    float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    #endregion

    #region WallSlide
    public Vector2 WallJumpClimb;
    public Vector2 WallJumpOff;
    public Vector2 WallLeap;

    float wallSlideSpeedMax = LPDefinitions.Character_WallSlideSpeed;
	float wallStickTime = LPDefinitions.Character_WallStickTime;
	float timeToWallUnstick;
    bool wallSliding;
    int wallDirX;
    #endregion

    #region PowerUps
    public PowerUp CurrentPowerUp;
    #endregion

	#region AliveSettings
	public bool IsAlive = true;
    #endregion

    #region DirectionalSettings
    Vector2 directionalInput;
    float lastXDirection = 1.0f;
    #endregion

    void Start() 
	{
		base.Start();

        Life = LPDefinitions.Character_MaxLife;

        OnCharacterDie += PlayerDied;

        LPBaseCollectable.OnCollectedSpecial += ReceivePowerUp;

        jumpCount = maxJumpCount;
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);

		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	void Update() 
{
        base.Update();
        
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2) + Mathf.Clamp(LPDefinitions.World_Gravity, 1, 100);	//	WorldGravity Attenuation

		maxJumpHeight = Mathf.Clamp(LPDefinitions.Character_MaxJumpHeight, 4, 10);
		minJumpHeight = Mathf.Clamp(LPDefinitions.Character_MinJumpHeight, 1, 5);

		wallSlideSpeedMax = Mathf.Clamp(LPDefinitions.Character_WallSlideSpeed, 1, 10);
		wallStickTime = Mathf.Clamp(LPDefinitions.Character_WallStickTime, 0, 5);

		if (IsRunning)
			moveSpeed = Mathf.Clamp(LPDefinitions.Character_Speed, 1, 20) * 2;
		else 
			moveSpeed = Mathf.Clamp(LPDefinitions.Character_Speed, 1, 20);

		CalculateVelocity ();
		HandleWallSliding ();

        Move(velocity * Time.deltaTime, directionalInput);
            
		if (collisions.above || collisions.below) {
			if (collisions.slidingDownMaxSlope) {
				velocity.y += collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}

        IsFalling = velocity.y <= 0.00000001f;        
    }

	public void SetDirectionalInput (Vector2 input, float motionX)
    {
        if (IsAlive) {
            directionalInput = input;

            if (motionX != 0.0f)
                lastXDirection = motionX;

            transform.localScale = new Vector3(lastXDirection, transform.localScale.y, transform.localScale.z);
        }
	}

	public void OnJumpInputDown() 
	{
        if (IsAlive) {
            if (wallSliding) {
                if (wallDirX == directionalInput.x) {
                    velocity.x = -wallDirX * WallJumpClimb.x;
                    velocity.y = WallJumpClimb.y;
                } else if (directionalInput.x == 0) {
                    velocity.x = -wallDirX * WallJumpOff.x;
                    velocity.y = WallJumpOff.y;
                } else {
                    velocity.x = -wallDirX * WallLeap.x;
                    velocity.y = WallLeap.y;
                }
            }

            if (collisions.below || jumpCount > 1) {

                jumpCount--;

                if (collisions.below) {
                    jumpCount = maxJumpCount;
                }

                if (collisions.slidingDownMaxSlope) {
                    if (directionalInput.x != -Mathf.Sign(collisions.slopeNormal.x)) { // not jumping against max slope
                        velocity.y = maxJumpVelocity * collisions.slopeNormal.y;
                        velocity.x = maxJumpVelocity * collisions.slopeNormal.x;
                    }
                } else {
                    velocity.y = maxJumpVelocity;
                }
            }
        }
	}

	public void OnJumpInputUp() 
	{
        if (IsAlive)
		    if (velocity.y > minJumpVelocity)
			    velocity.y = minJumpVelocity;
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
                                    Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne),
                                    groundSlideFactor);

        velocity.y += gravity * Time.deltaTime;
	}
    
    public void AddImpulseUp()
    {
        velocity = new Vector3(velocity.x, LPDefinitions.Character_ImpulseUp, velocity.z);
    }

    public void AddImpulseBack()
    {
        velocity = new Vector3((velocity.x > 0? -LPDefinitions.Character_ImpulseBack: LPDefinitions.Character_ImpulseBack), velocity.y, velocity.z);
    }

    public void ReceivePowerUp(PowerUp powerUp)
    {
        if(powerUp == PowerUp.DoubleJump)             
            maxJumpCount = LPDefinitions.Character_MaxJumpCount = 2;

        if (powerUp == PowerUp.Magnet) {

            CurrentPowerUp = powerUp;

            Invoke("ResetPowerUp", LPDefinitions.Magnet_LastDuration);
        }
    }

    void ResetPowerUp()
    {
        CurrentPowerUp = PowerUp.None;
    }

    public void PlayerDied()
    {
        if (IsAlive) {

            IsAlive = false;
            base.TurnOffCollisions();
            AddImpulseUp();
            AddImpulseBack();
        }
    }
}