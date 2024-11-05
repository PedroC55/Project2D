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
	private bool isDashing = false;

	public float movemntInputX, movementInputY, dashingPowerX, dashingPowerY;
	private bool isWallJumping = false;


	private int slowPercentage;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		float currentSpeed = moveSpeed;

		if (isDashing)
		{
			return;
		}

		if (slowPercentage > 0)
		{
			currentSpeed -= moveSpeed * (slowPercentage / 100f);
		}
		if (isWallJumping)
        {
			return;
		}

		else if (wallSlidingSpeed != 0)
        {
			rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
		}

		rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, rb2d.velocity.y);
		

	}

	public void ResetDash()
    {
		isDashing = false;
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

	public void WallJump(Vector2 wallJF)
    {
		wallJumpForce = wallJF;
		isWallJumping = true;
		rb2d.Sleep();
		rb2d.AddForce(wallJumpForce, ForceMode2D.Impulse);
	}

	public void IsExecutingDash()
    {
		isDashing = true;
    }


    public void ResetWallJump()
	{
		isWallJumping = false;
		wallJumpForce = Vector2.zero;
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