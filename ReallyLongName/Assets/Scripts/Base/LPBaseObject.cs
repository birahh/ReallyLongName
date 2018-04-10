using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider2D))]
public class LPBaseObject : MonoBehaviour 
{
	public Collider2D collider;

	public bool CanDie;
	public bool ShouldRespawn;
	public float Speed;

	private Animator animator;

	public void Start () 
	{
		animator = GetComponentInChildren<Animator>();
		collider = GetComponentInChildren<Collider2D>();
	}

	void Update () 
	{
		
	}

	protected void RunAnimationByName(string animationName)
	{
		if (animator) {
			animator.Play(animationName);
		}
	}
}
