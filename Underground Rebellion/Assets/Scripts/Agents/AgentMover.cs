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
	public float movemntInputX, dashingPower;

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
		//O melhor seria fazer verificações do tipo,
		//if(isWallJumping){
		//	aplica os calculos no rb2d.velocity baseado no walljumping
		//	return;
		//}
		//Fazer isso para todos, no caso, wallJumping, Sliding e Dashing
		//No fim de tudo , caso não esteja fazendo nenhuma dessas 3 ações
		//Colocar para andar normal: rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, rb2d.velocity.y);

		if (wallJumpForce.x != 0)
        {
			ApplyForce(wallJumpForce);
			return;
		}
		else if (wallSlidingSpeed != 0)
        {
			rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
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

			if((new[] { GravityDirection.Down, GravityDirection.Up }).Contains(agentWM.GetGravityDirection()))
				xSpeed = MovementInput.x * currentSpeed;
			else
				ySpeed = MovementInput.y * currentSpeed;

			rb2d.velocity = new Vector2(xSpeed, ySpeed);
		}


		if (movemntInputX != 0)
        {
			Dash(movemntInputX, dashingPower);
        }
	}

	public float GetCurrentSpeed()
	{
		return currentSpeed;
	}

	public void ResetDash()
    {
		movemntInputX = 0f;
		dashingPower = 0f;
    }
	public void Dash(float movemntInput, float dashingPower)
    {
		rb2d.velocity = new Vector2(movemntInput * dashingPower, 0f);
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