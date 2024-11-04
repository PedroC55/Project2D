using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
	private Rigidbody2D rb2d;

	public Vector2 wallJumpForce;
	[SerializeField]
	private float moveSpeed;
	public Vector2 MovementInput { get; set; }

	public float jumpForce, wallSlidingSpeed;
	public float movemntInputX, movementInputY, dashingPowerX, dashingPowerY;

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

		if (wallJumpForce.x != 0)
        {
			ApplyForce(wallJumpForce);
		}
		else if (wallSlidingSpeed != 0)
        {
			rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
		}

        else if (MovementInput.x != 0)
        {
			rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, rb2d.velocity.y);
		}

		if (dashingPowerX != 0)
        {
			Dash(movemntInputX,movementInputY, dashingPowerX, dashingPowerY);
        }
	}

	public void ResetDash()
    {
		movemntInputX = 0f;
		movementInputY = 0f;
		dashingPowerX = 0f;
		dashingPowerY = 0f;
		rb2d.velocity = Vector2.zero;
    }
	public void Dash(float movemntInputX, float movementInputY, float dashingPowerX, float dashingPowerY)
    {
		rb2d.velocity = new Vector2(movemntInputX * dashingPowerX, movementInputY * dashingPowerY);
	}

	public void ApplyForce(Vector2 direction)
    {
		rb2d.AddForce(direction, ForceMode2D.Impulse);
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