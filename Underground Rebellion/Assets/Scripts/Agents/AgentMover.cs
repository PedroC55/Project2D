using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
	private Rigidbody2D rb2d;

	[SerializeField]
	private float maxSpeed = 2, acceleration = 50, deacceleration = 100;
	[SerializeField]
	private float currentSpeed = 0;
	private Vector2 oldMovementInput;
	public Vector2 wallJumpForce;

	[SerializeField] private float moveSpeed = 7f;
	public Vector2 MovementInput { get; set; }

	public float jumpForce, wallSlidingSpeed;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		Debug.Log(wallSlidingSpeed);
		rb2d.velocity = new Vector2(wallJumpForce.x == 0 ? MovementInput.x * moveSpeed : -MovementInput.x * wallJumpForce.x, wallSlidingSpeed != 0 ? Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue) :(jumpForce > 0 ? jumpForce : rb2d.velocity.y));
		//if (MovementInput.magnitude > 0)
		//{
		//	oldMovementInput = MovementInput;
		//	currentSpeed += acceleration * maxSpeed * Time.deltaTime;
		//}
		//else
		//{
		//	currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
		//}
		//currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
		//rb2d.velocity = oldMovementInput * currentSpeed;

	}

	public void StopMoving()
	{
		rb2d.bodyType = RigidbodyType2D.Static;
	}
}