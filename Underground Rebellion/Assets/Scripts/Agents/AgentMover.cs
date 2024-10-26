using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
	private Rigidbody2D rb2d;

	[SerializeField]
	private float moveSpeed;
	public Vector2 MovementInput { get; set; }

	public float jumpForce;

	private int slowPercentage;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		float currentSpeed = moveSpeed;
		
		if (slowPercentage > 0)
		{
			currentSpeed -= moveSpeed * (slowPercentage / 100f);
		}

		rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, jumpForce > 0 ? jumpForce : rb2d.velocity.y);
	}

	public void StopMoving()
	{
		rb2d.bodyType = RigidbodyType2D.Static;
	}

	public void SlowMovement(int percentage)
	{
		if (slowPercentage < percentage || percentage == 0)
			slowPercentage = percentage;
	}
}