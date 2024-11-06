using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
	private Rigidbody2D rb2d;
	private AgentAnimations agentAnimations;
	private AgentMover agentMover;
	private Health health;
	public Transform wallCheck;
	public float jumpForce, wallSlidingSpeed;

	public Vector2 wallJumpForce;


	private Vector2 lookDirection, movementInput;
	public Vector2 MovementInput { get => movementInput; set => movementInput = value; }



	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		agentAnimations = GetComponentInChildren<AgentAnimations>();
		agentMover = GetComponent<AgentMover>();
		health = GetComponent<Health>();
		if (agentMover == null)
		{
			Debug.LogError("AgentMover is not assigned or found in Agent.");
		}
	}

	protected void Update()
	{
		agentMover.MovementInput = movementInput;
		if (wallCheck != null)
		{
			agentMover.wallSlidingSpeed = wallSlidingSpeed;
			agentAnimations.wallCheck = wallCheck;
		}
		AnimateCharacter();
	}

	public void IsExecutingDash()
    {
		agentMover.IsExecutingDash();
    }

	public void WallJump(Vector2 wallJF)
    {
		agentMover.WallJump(wallJF);
		//agentAnimations.ChangeLookDirection(wallJF);
    }

	public void ResetWallJump()
    {
		agentMover.ResetWallJump();
    }
	public void ApplyForce(Vector2 direction)
	{
		agentMover.ApplyForce(direction);
	}
	public void FaceDirection(Vector2 direction)
	{
		lookDirection = direction;
		agentAnimations.LookDirection(direction);
	}

	public void PerformAttack(string triggerName)
	{
		agentAnimations.AttackAnimation(triggerName);
	}

	public void Died()
	{
		agentMover.StopMoving();
		agentAnimations.DeathAnimation();
	}

	public void SlowMovement(int slowPercentage)
	{
		agentMover.SlowMovement(slowPercentage);
	}

	public void GetHit(int damage, GameObject sender)
	{
		health.GetHit(damage);
	}

	public void ResetDash()
    {

		agentMover.ResetDash();
    }

	public void Dash(float dashingPowerX,float dashingPowerY ,Vector2 movementInput)
    {
		float direction_x = agentAnimations.GetDirection();

		agentMover.Dash(direction_x, movementInput.y, dashingPowerX, dashingPowerY);
		


    }

	public void StunAnimation()
	{
		agentAnimations.StunAnimation();
	}

	public float GetCurrentSpeed()
	{
		return agentMover.GetCurrentSpeed();
	}

	private void AnimateCharacter()
	{
		if (movementInput.x > 0 || movementInput.x < 0)
		{
			agentAnimations.WalkingAnimation(movementInput);
		}
		else
		{
			agentAnimations.IdleAnimation();
		}

		// Aten��o!! Fazer verifica��o do salto depois da corrida uma vez que o salto tem prioridade!!
		if (rb2d.velocity.y > .1f)
		{
			agentAnimations.JumpingAnimation();
		}
		else if (rb2d.velocity.y < -.1f)
		{
			agentAnimations.FallingAnimation();
		}
		
			
	}
}