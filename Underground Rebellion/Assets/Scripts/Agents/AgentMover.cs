using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class AgentMover : MonoBehaviour
{
	private Rigidbody2D rb2d;

	[SerializeField]
	private float moveSpeed;
	private float currentSpeed;
	public Vector2 MovementInput { get; set; }

	private bool isDead = false;

	public float WallSlidingSpeed { get; set; }
	private bool isDashing = false;

	private bool isWallJumping = false;

	private int slowPercentage;

	private float gravityScale;
	
	private bool canWalkWalls = false;
	private WallMovement agentWM;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		gravityScale = rb2d.gravityScale;
		currentSpeed = moveSpeed;

		agentWM = GetComponent<WallMovement>();
		canWalkWalls = agentWM ? true : false;
		rb2d.gravityScale = agentWM ? 0 : rb2d.gravityScale;
	}

	private void FixedUpdate()
	{
		if (isDead || isDashing || isWallJumping || rb2d.bodyType == RigidbodyType2D.Static)
		{
			return;
		}

		else if (WallSlidingSpeed != 0)
		{
			rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, Mathf.Clamp(rb2d.velocity.y, -WallSlidingSpeed, float.MaxValue));
			return;
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


		if (rb2d.velocity.y < 0)
		{
			rb2d.gravityScale = gravityScale * 1.5f;
		}
		else
		{
			rb2d.gravityScale = gravityScale;
		}

	}
	public float GetCurrentSpeed()
	{
		return currentSpeed;
	}

	public void Jump(Vector2 direction)
	{
		ApplyForce(direction);
	}

	public void Dash(float movemntInputX, float movementInputY, float dashingPowerX, float dashingPowerY)
    {
		rb2d.velocity = new Vector2(movemntInputX * dashingPowerX, movementInputY * dashingPowerY);
	}
	public void ResetDash()
    {
		rb2d.velocity = Vector2.zero;
		isDashing = false;
    }

	public void WallJump(Vector2 wallJF)
    {
		isWallJumping = true;
		rb2d.Sleep();
		rb2d.AddForce(wallJF, ForceMode2D.Impulse);
	}

	public void IsExecutingDash()
    {
		isDashing = true;
    }

    public void ResetWallJump()
	{
		isWallJumping = false;
	}

	public void ApplyForce(Vector2 direction)
    {
		rb2d.AddForce(direction, ForceMode2D.Impulse);
	}

	public void AgentDied()
	{
		rb2d.bodyType = RigidbodyType2D.Static;
		isDead = true;
	}

	public void Reset()
	{
		rb2d.bodyType = RigidbodyType2D.Dynamic;
		isDead = false;
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