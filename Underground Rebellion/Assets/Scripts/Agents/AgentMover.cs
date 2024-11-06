using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
	private Rigidbody2D rb2d;

	public Vector2 wallJumpForce;
	[SerializeField]
	private float moveSpeed;
	private float currentSpeed;
	public Vector2 MovementInput { get; set; }

	public float jumpForce, wallSlidingSpeed;
	private bool isDashing = false;

	public float movemntInputX, movementInputY, dashingPowerX, dashingPowerY;
	private bool isWallJumping = false;


	private int slowPercentage;

	
	private bool canWalkWalls = false;
	private WallMovement agentWM;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		
		currentSpeed = moveSpeed;

		agentWM = GetComponent<WallMovement>();
		canWalkWalls = agentWM ? true : false;
		rb2d.gravityScale = agentWM ? 0 : rb2d.gravityScale;
	}

	private void FixedUpdate()
	{

		if (isDashing)
		{
			return;
		}

		if (isWallJumping)
		{
			return;
		}

		else if (wallSlidingSpeed != 0)
		{
			rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
		}

		if (!canWalkWalls)
		{
			rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, rb2d.velocity.y);
		}
		else
		{
			float xSpeed = rb2d.velocity.x;
			float ySpeed = rb2d.velocity.y;

			if ((new[] { GravityDirection.Down, GravityDirection.Up }).Contains(agentWM.GetGravityDirection()))
				xSpeed = MovementInput.x * currentSpeed;
			else
				ySpeed = MovementInput.y * currentSpeed;

			rb2d.velocity = new Vector2(xSpeed, ySpeed);
		}
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
	public float GetCurrentSpeed()
	{
		return currentSpeed;
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

		currentSpeed = moveSpeed;
		if (slowPercentage > 0)
		{
			currentSpeed -= moveSpeed * (slowPercentage / 100f);
		}
	}
}