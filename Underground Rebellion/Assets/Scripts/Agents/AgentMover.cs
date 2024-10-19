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
		//if (wallJumpForce.x != 0)
        //{
		//	Debug.Log("1");
		//	rb2d.velocity = new Vector2(MovementInput.x * wallJumpForce.x, wallJumpForce.y);
		//
		//}
		if (wallSlidingSpeed != 0)
        {
			//Debug.Log("2");
			rb2d.velocity = new Vector2(MovementInput.x * moveSpeed, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));

		}
		//else if (jumpForce > 0)
        //{
		//	Debug.Log("3");
		//	rb2d.velocity = new Vector2(MovementInput.x * moveSpeed, jumpForce);
		//
		//}
        else
        {
			
			rb2d.velocity = new Vector2(MovementInput.x * moveSpeed, rb2d.velocity.y);
		}


		//rb2d.velocity = new Vector2(wallJumpForce.x == 0 ? MovementInput.x * moveSpeed : MovementInput.x * wallJumpForce.x, wallSlidingSpeed != 0 ? Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue) :(jumpForce > 0 ? jumpForce : rb2d.velocity.y));
		//Debug.Log(rb2d.velocity);
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

	public void ApplyForce(Vector2 direction)
    {
		//Debug.Log(direction);
		rb2d.AddForce(direction, ForceMode2D.Impulse);
		Debug.Log(rb2d.velocity);
	}
	public void StopMoving()
	{
		rb2d.bodyType = RigidbodyType2D.Static;
	}
}