using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPBaseEnemy : LPBaseObject
{
	public LayerMask VictimMask;

	public Vector3[] LocalWaypoints;
	Vector3[] globalWaypoints;

	public bool IsCyclic;
	public bool ShouldPlayFromStart;
	public float WaitTime;

	[Range(0,2)]
	public float EaseAmount;

    private int fromWaypointIndex;
    private int cycleIteration;
    private int cycleLimit;
    protected float percentBetweenWaypoints;
    private float nextMoveTime;
    private bool shouldPlay;
    protected bool dontComeBack;
    private Vector3 initialPosition;
    private int lenght = 0;
	private bool isPlayerActivated = false;

	public delegate void EnemyGotHit();
	public static event EnemyGotHit OnEnemyGotHit;

	public delegate void EnemyHitFloor();
	public static event EnemyHitFloor OnEnemyHitFloor;

	public ParticleSystem[] OnDieParticles;
	public ParticleSystem[] OnHitFloorParticles;
	public TrailRenderer Trail;

    public void Start ()
    {
        initialPosition = transform.position;

		base.Start();

        cycleLimit = int.MaxValue;
        cycleIteration = 0;

        shouldPlay = ShouldPlayFromStart;

        lenght = LocalWaypoints.Length;
        
        globalWaypoints = new Vector3[lenght];

        for (int i = 0; i < lenght; i++) {
            globalWaypoints[i] = LocalWaypoints[i] + transform.position;
        }
    }

	public void Update ()
	{
		if (shouldPlay) {
            
            if (cycleIteration < cycleLimit) {

                Vector3 velocity = CalculateMovement();
                transform.Translate(velocity);

            } else {

                shouldPlay = false;
                cycleIteration = -1;
            }
		}
	}

    #region Activate/Reset Enemy
    public void Activate()
	{
		shouldPlay = true;
	}

    public void Activate(int RepetitionCount)
    {
        cycleLimit = RepetitionCount;
        shouldPlay = true;
    }

    public void Activate(float delay)
    {
		isPlayerActivated = true;
        Invoke("Activate", delay);
    }

    public void Reset()
    {
        transform.position = initialPosition;

        cycleLimit = int.MaxValue;
        cycleIteration = 0;

        shouldPlay = ShouldPlayFromStart;

        globalWaypoints = new Vector3[lenght];
        percentBetweenWaypoints = 0;
        fromWaypointIndex = 0;

        for (int i = 0; i < lenght; i++)
        {
            globalWaypoints[i] = LocalWaypoints[i] + transform.position;
        }
    }
    #endregion

    #region Movement Methods
    float Ease(float x) {
		float a = EaseAmount + 1;
		return Mathf.Pow(x,a) / (Mathf.Pow(x,a) + Mathf.Pow(1-x,a));
	}

	Vector3 CalculateMovement()
	{
		if (Time.time < nextMoveTime) {
			return Vector3.zero;
		}

        //  Just Go, Never Get Back
        if (dontComeBack && percentBetweenWaypoints >= 0.9f) {
            shouldPlay = false;
            Invoke("Reset", 0.1f);
        }

		fromWaypointIndex %= globalWaypoints.Length;
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance (globalWaypoints [fromWaypointIndex], globalWaypoints [toWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * Speed/distanceBetweenWaypoints;
		percentBetweenWaypoints = Mathf.Clamp01 (percentBetweenWaypoints);
		float easedPercentBetweenWaypoints = Ease (percentBetweenWaypoints);
        
		Vector3 newPos = Vector3.Lerp (globalWaypoints [fromWaypointIndex], globalWaypoints [toWaypointIndex], easedPercentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1) {

			percentBetweenWaypoints = 0;
			fromWaypointIndex ++;

			if (!IsCyclic) {
				if (fromWaypointIndex >= globalWaypoints.Length-1) {
					fromWaypointIndex = 0;
					System.Array.Reverse(globalWaypoints);

					if (isPlayerActivated && cycleIteration% 2 != 0)
						HitGround();
					else 
						if (Trail != null)
							Trail.GetComponent<Renderer>().enabled = true;
						

                    cycleIteration++;
				}
			}

			nextMoveTime = Time.time + WaitTime;
		}

		return newPos - transform.position;
	}
    #endregion

	void HitGround()
	{
		foreach (ParticleSystem particle in OnHitFloorParticles) {
			particle.Play();
		}

		if (OnEnemyHitFloor != null) {

			OnEnemyHitFloor();
		}
	}

	void GotHit()
	{
		foreach (ParticleSystem particle in OnDieParticles) {
			particle.Play();
		}

		if (Trail != null)
			Trail.GetComponent<Renderer>().enabled = false;


		if (OnEnemyGotHit != null) {
			
			OnEnemyGotHit();
		}
	}

    #region Collisions
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Equals("Player")) {

            LPPlayableCharacter player = coll.GetComponentInParent<LPPlayableCharacter>();

            if (player.CanGetHit) {

                float heightDiff = player.transform.position.y - transform.position.y;

                if (player.IsFalling && CanDie && ( heightDiff >= 0.08f)) {
					
					GotHit();
					player.AddImpulseUp();
					GetComponent<Renderer>().enabled = false;
					GetComponent<Collider2D>().enabled = false;
					Invoke("DestroySelf", 1.0f);
                    
                } else {

                    player.AddImpulseUp();
                    player.Hit();
                    player.AddImpulseBack();
                }
            }
        }
    }
    #endregion

	void DestroySelf()
	{
		GameObject.Destroy(gameObject);
	}

    #region UI On Editor
    void OnDrawGizmos() 
	{
		if (LocalWaypoints != null) {
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i =0; i < lenght; i ++) {
				Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i] : LocalWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}
    #endregion
}
