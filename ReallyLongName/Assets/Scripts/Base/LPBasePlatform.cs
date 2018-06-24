using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LPBasePlatform : LPRaycastController 
{
	public LayerMask passengerMask;

	public Vector3[] localWaypoints;
	Vector3[] globalWaypoints;
    Vector3 velocity;

    public float speed;
	public bool cyclic;
	public float waitTime;

	[Range(0,2)]
	public float easeAmount;
	public float AnimationDelay = 0.5f;

    private bool isOn = true;
    private bool isFalling = false;
    private int fromWaypointIndex;
    private float percentBetweenWaypoints;
    private float nextMoveTime;
    private float gravity;
    private Animator animator;
    
	public Dictionary<Transform, LP2DController> passengerDictionary = new Dictionary<Transform, LP2DController>();
	List<PassengerMovement> passengerMovement;

	public override void Start () 
	{
		base.Start ();

        animator = GetComponent<Animator>();

        if(animator)
            Invoke("startAnimation", AnimationDelay);

		speed = LPDefinitions.Platform_Speed;

		globalWaypoints = new Vector3[localWaypoints.Length];

		for (int i =0; i < localWaypoints.Length; i++) {
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
	}

	public void Update () 
	{
        if(isOn) {
            
		    UpdateRaycastOrigins ();

            velocity = CalculatePlatformMovement();

		    if (localWaypoints.Length > 0) {			

			    CalculatePassengerMovement(velocity);

			    MovePassengers (true);
			    transform.Translate (velocity);
			    MovePassengers (false);
            }
        }

        if(isFalling) {
            velocity.y += gravity * Time.deltaTime;

            transform.Translate(velocity);
        }
    }

    #region General Activation
    public void Activate()
    {
        if(this.GetType() == typeof(LPPlatformMoving))
            TurnOn();

        if(this.GetType() == typeof(LPPlatformDrop))
            Invoke("Fall", LPDefinitions.PlatformFalling_TimeBeforeActivate);

        if(this.GetType() == typeof(LPPlatformGlitch))
            Invoke("Glitch", LPDefinitions.PlatformGlitch_TimeBeforeActivate);
    }
    #endregion

	#region Spike Settings	
	void startAnimation()
	{
		print("Activate - ");
		animator.Play("Activate");
	}
	#endregion

    #region On/Off Settings
    public void TurnOff()
    {
        isOn = false;
    }

    public void TurnOn()
    {
        isOn = true;
    }
    #endregion

    #region Falling Mechanic
    public void Fall()
    {
        TurnOff();

        isFalling = true;

        gravity = -Mathf.Clamp(LPDefinitions.World_Gravity, 1, 100);

        Invoke("Desapear", LPDefinitions.Platform_VanishingTime);
    }

    public void Desapear()
    {
        //  Maybe apply a LERP in transparency

        GameObject.Destroy(gameObject);
    }
    #endregion

    #region Glitch Mechanic
    public void Glitch()
    {
        TurnOff();

        SetGlitchOn();

        Invoke("SetGlitchOff", LPDefinitions.PlatformGlitch_TimeToReset);
    }

    public void SetGlitchOn()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void SetGlitchOff()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
    #endregion
    
    #region Movement Mechanic    
    float Ease(float x) 
	{
		float a = easeAmount + 1;
		return Mathf.Pow(x,a) / (Mathf.Pow(x,a) + Mathf.Pow(1-x,a));
	}

	Vector3 CalculatePlatformMovement() {

		if (Time.time < nextMoveTime) {
			return Vector3.zero;
		}

		if (globalWaypoints.Length != 0) {

			fromWaypointIndex %= globalWaypoints.Length;

			int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
			float distanceBetweenWaypoints = Vector3.Distance (globalWaypoints [fromWaypointIndex], globalWaypoints [toWaypointIndex]);

			percentBetweenWaypoints += Time.deltaTime * speed/distanceBetweenWaypoints;
			percentBetweenWaypoints = Mathf.Clamp01 (percentBetweenWaypoints);
			float easedPercentBetweenWaypoints = Ease (percentBetweenWaypoints);

			Vector3 newPos = Vector3.Lerp (globalWaypoints [fromWaypointIndex], globalWaypoints [toWaypointIndex], easedPercentBetweenWaypoints);

			if (percentBetweenWaypoints >= 1) {
				percentBetweenWaypoints = 0;
				fromWaypointIndex ++;

				if (!cyclic) {
					if (fromWaypointIndex >= globalWaypoints.Length-1) {
						fromWaypointIndex = 0;
						System.Array.Reverse(globalWaypoints);
					}
				}
				nextMoveTime = Time.time + waitTime;
			}

			return newPos - transform.position;
		}

		return Vector3.zero;
	}

    #region PassengerMovement
    void MovePassengers(bool beforeMovePlatform) 
	{
		foreach (PassengerMovement passenger in passengerMovement) {
			if (!passengerDictionary.ContainsKey(passenger.transform)) {
				passengerDictionary.Add(passenger.transform,passenger.transform.GetComponent<LP2DController>());
			}

			if (passenger.moveBeforePlatform == beforeMovePlatform) {
				passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
			}
		}
	}

	void CalculatePassengerMovement(Vector3 velocity) 
	{
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();
		passengerMovement = new List<PassengerMovement> ();

		float directionX = Mathf.Sign (velocity.x);
		float directionY = Mathf.Sign (velocity.y);

		// Vertically moving platform
		if (velocity.y != 0) {
			float rayLength = Mathf.Abs (velocity.y) + skinWidth;

			for (int i = 0; i < verticalRayCount; i ++) {
				Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

				if (hit && hit.distance != 0) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = (directionY == 1)?velocity.x:0;
						float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

						passengerMovement.Add(new PassengerMovement(hit.transform,new Vector3(pushX,pushY), directionY == 1, true));
					}
				}
			}
		}

		// Horizontally moving platform
		if (velocity.x != 0) {
			float rayLength = Mathf.Abs (velocity.x) + skinWidth;

			for (int i = 0; i < horizontalRayCount; i ++) {
				Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

				if (hit && hit.distance != 0) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
						float pushY = -skinWidth;

						passengerMovement.Add(new PassengerMovement(hit.transform,new Vector3(pushX,pushY), false, true));
					}
				}
			}
		}

		// Passenger on top of a horizontally or downward moving platform
		if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {
			float rayLength = skinWidth * 2;

			for (int i = 0; i < verticalRayCount; i ++) {
				Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

				if (hit && hit.distance != 0) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;

						passengerMovement.Add(new PassengerMovement(hit.transform,new Vector3(pushX,pushY), true, false));
					}
				}
			}
		}
	}

	struct PassengerMovement {
		public Transform transform;
		public Vector3 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;

		public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform) {
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
		}
	}
    #endregion
    #endregion

    #region EDITOR Stuff
    void OnDrawGizmos() {
		if (localWaypoints != null) {
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i =0; i < localWaypoints.Length; i ++) {
				Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
    }
    #endregion
}
