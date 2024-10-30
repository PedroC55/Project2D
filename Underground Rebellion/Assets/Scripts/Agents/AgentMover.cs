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
	public float movemntInputX, dashingPower;

	private int slowPercentage;

	public bool canWalkWalls = false;

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

		//O melhor seria fazer verifica��es do tipo,
		//if(isWallJumping){
		//	aplica os calculos no rb2d.velocity baseado no walljumping
		//	return;
		//}
		//Fazer isso para todos, no caso, wallJumping, Sliding e Dashing
		//No fim de tudo , caso n�o esteja fazendo nenhuma dessas 3 a��es
		//Colocar para andar normal: rb2d.velocity = new Vector2(MovementInput.x * currentSpeed, rb2d.velocity.y);

		if (wallJumpForce.x != 0)
        {
			ApplyForce(wallJumpForce);
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
			rb2d.velocity = MovementInput * currentSpeed;
		}


		if (movemntInputX != 0)
        {
			Dash(movemntInputX, dashingPower);
			
        }
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
	}
}