﻿using System.Collections;
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

	int fromWaypointIndex;
	float percentBetweenWaypoints;
	float nextMoveTime;
	bool shouldPlay;
    
	public void Start ()
    {
		base.Start();

		shouldPlay = ShouldPlayFromStart;

		globalWaypoints = new Vector3[LocalWaypoints.Length];

		for (int i =0; i < LocalWaypoints.Length; i++) {
			globalWaypoints[i] = LocalWaypoints[i] + transform.position;
		}
	}

	public void Update ()
	{
		if (shouldPlay) {
			
			Vector3 velocity = CalculateMovement();

			transform.Translate (velocity);
		}
	}

	public void Activate()
	{
		shouldPlay = true;
	}

	float Ease(float x) {
		float a = EaseAmount + 1;
		return Mathf.Pow(x,a) / (Mathf.Pow(x,a) + Mathf.Pow(1-x,a));
	}

	Vector3 CalculateMovement()
	{
		if (Time.time < nextMoveTime) {
			return Vector3.zero;
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
				}
			}

			nextMoveTime = Time.time + WaitTime;
		}

		return newPos - transform.position;
	}

	void OnDrawGizmos() 
	{
		if (LocalWaypoints != null) {
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i =0; i < LocalWaypoints.Length; i ++) {
				Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i] : LocalWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}
}
