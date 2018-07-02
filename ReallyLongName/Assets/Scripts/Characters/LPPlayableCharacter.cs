using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPPlayableCharacter : LPBaseCharacter
{
    #region GroundMovementation
    public bool IsRunning;
    public bool IsWalking;
    public bool IsJumping;
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
	bool finishPose = false;
    #endregion

    #region DirectionalSettings
    Vector2 directionalInput;
    float lastXDirection = 1.0f;
    #endregion

    #region ParticlesSettings
    public ParticleSystem RunEmitter;
    public ParticleSystem JumpUpEmitter;
    public ParticleSystem JumpDownEmitter;
    public ParticleSystem WallSlideEmitter;

    bool wasJumpUpEmitterOn = false;
    bool wasJumpDownEmitterOn = false;
	bool hadCollisionsBelow = false;
	bool wasFallingDown = false;
    #endregion

    private Animator animator;

    void Start() 
	{
		base.Start();

        animator = GetComponent<Animator>();

        Life = LPDefinitions.Character_MaxLife;

        OnCharacterDie += PlayerDied;
        OnCharacterFinishLevel += FinishPose;

        LPBaseCollectable.OnCollectedSpecial += ReceivePowerUp;

        jumpCount = maxJumpCount;
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);

		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	void Update() 
	{
		if (IsActive) {
			
	        base.Update();

			gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2) + Mathf.Clamp(LPDefinitions.World_Gravity, 1, 100);  //	WorldGravity Attenuation

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
					velocity.y += collisions.slopeNormal.y * - gravity * Time.deltaTime;
				} else {
					velocity.y = 0;
				}
			}

	        IsFalling = velocity.y < -0.00000001f;
	        IsWalking = (velocity.x > 0.05f)||(velocity.x < -0.05f);
	        IsJumping = (velocity.y > 0.05f)||(velocity.y < -0.05f);

	        #region ParticleSystemHandle
			if (IsFalling && !wasFallingDown)
				wasFallingDown = true;

			if (collisions.below && !hadCollisionsBelow)
				hadCollisionsBelow = true;

	        //  RunEmitter
	        if (Mathf.Abs(velocity.x) > 0.5f && collisions.below) {
	            RunEmitter.Play();
	        } else {
	            RunEmitter.Stop();
	        }

	        //  JumpUpEmitter
			if (velocity.y > 0.00000001f && hadCollisionsBelow && !wasJumpUpEmitterOn) {
	            JumpUpEmitter.Play();
				hadCollisionsBelow = false;
	            wasJumpUpEmitterOn = true;
	            animator.Play("Jump");
	        } else if(wasJumpUpEmitterOn) {
	            Invoke("JumpUpEmitterStop", .5f);     
	        }

	        //  JumpDownEmitter
			if (wasFallingDown && collisions.below && !wasJumpDownEmitterOn) {
	            JumpDownEmitter.Play();
				wasFallingDown = false;
	            wasJumpDownEmitterOn = true;
	            animator.Play("Idle");
	        } else if(wasJumpDownEmitterOn) {
	            Invoke("JumpDownEmitterStop", .05f);            
	        }

	        //  WallSlideEmitter
			if (wallSliding)
	            WallSlideEmitter.Play();
	        else
	            WallSlideEmitter.Stop();
	        #endregion

	        #region Platforms Activate
	        if(collisions.below)
	            if(collisions.objectTag.Equals("Platform"))
	                if(collisions.objectGameObject.GetComponent<LPBasePlatform>())
	                    collisions.objectGameObject.GetComponent<LPBasePlatform>().Activate();
	        #endregion

	        #region Animation Settings
	        animator.SetBool("isWallSliding", wallSliding);
	        animator.SetBool("isWalking", IsWalking);
	        animator.SetBool("isJumping", IsJumping);
	        // print(IsWalking+", "+IsJumping+", "+wallSliding);
	        #endregion
		} else if (finishPose) {
			
			float newX = Mathf.Lerp(transform.position.x, endZonePosition.x, 0.1f);
			float newY = Mathf.Lerp(transform.position.y, endZonePosition.y, 0.1f);
			float newZ = Mathf.Lerp(transform.position.z, endZonePosition.z, 0.1f);

			transform.position = new Vector3(newX, newY, newZ);
			animator.speed = Mathf.Lerp(animator.speed, 3.0f, 0.8f);

			float newYa = Mathf.Lerp(transform.localScale.y, 0, 0.05f);

			transform.localScale = new Vector3(newYa, newYa, newYa);

		}
    }

    void JumpDownEmitterStop()
    {
        wasJumpDownEmitterOn = false;
        JumpDownEmitter.Stop();
    }

    void JumpUpEmitterStop()
    {        
        wasJumpUpEmitterOn = false;
        JumpUpEmitter.Stop();
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

			Jump();

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

	public void AddIpulseTowardsCamera()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
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
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

        foreach (Collider2D coll in colliders) {
            coll.enabled = false;
        }

        print("Before");

		if (IsAlive) {
            print("ONCE");
            base.TurnOffCollisions();

			velocity = Vector3.zero;
			IsAlive = false;
			IsActive = false;

			animator.SetBool("isDead", true);

			Invoke("AddImpulses", 0.5f);
        }
    }

	void AddImpulses()
	{
		IsActive = true;
		RemoveDelegates();
		AddImpulseUp();
		AddIpulseTowardsCamera();
		AddImpulseBack();
	}

    public void FinishPose()
	{
		IsActive = false;
		finishPose = true;
		RemoveDelegates();
		animator.SetBool("finishPose", true);
    }

	void RemoveDelegates()
	{
		OnCharacterDie -= PlayerDied;
		OnCharacterFinishLevel -= FinishPose;

		LPBaseCollectable.OnCollectedSpecial -= ReceivePowerUp;
	}
}