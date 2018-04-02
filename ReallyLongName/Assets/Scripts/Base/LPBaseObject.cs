using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class LPBaseObject : MonoBehaviour 
{
	public BoxCollider2D collider;

	public bool CanDie;
	public bool ShouldRespawn;

	public float Speed;

	private Animator animator;

	void Start () 
	{
		animator = GetComponentInChildren<Animator>();
		collider = GetComponentInChildren<BoxCollider2D>();
	}


	void Update () 
	{
		
	}

	protected void RunAnimator(string animationName)
	{
		if (animator) {
			animator.Play(animationName);
		}
	}
}
