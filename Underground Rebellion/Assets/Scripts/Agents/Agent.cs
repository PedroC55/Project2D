using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
	private Rigidbody2D rb2d;
	private AgentAnimations agentAnimations;
	private AgentMover agentMover;
	private Health health;
	public Transform wallCheck;
	public float wallSlidingSpeed;

	public Vector2 wallJumpForce;

	private Vector2 movementInput;
	public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

	private bool isStunned = false;

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
			agentMover.WallSlidingSpeed = wallSlidingSpeed;
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
		agentAnimations.LookDirection(direction);
	}

	public void PerformAttack(string triggerName)
	{
		agentAnimations.AttackAnimation(triggerName);
	}

	public void Died()
	{
		isStunned = false;
		agentMover.AgentDied();
		agentAnimations.DeathAnimation();
	}

	public void Disappear()
	{
		agentMover.AgentDied();
		agentAnimations.DisappearAnimation();
	}

	public void ResetAgent(bool isPlayer)
	{
		agentMover.Reset();
		if (health != null)
			health.ResetHealth();

		if (isPlayer)
			CanvasEvent.UpdateHealth(health.GetMaxHealth());
	}

	public void RespawnAgent()
	{
		agentAnimations.RespawnAnimation();
		agentMover.Reset();
		if (health != null)
			health.ResetHealth();
	}

	public void Recovered()
	{
		isStunned = false;
	}

	public void Stunned()
	{
		isStunned = true;
	}

	public void SlowMovement(int slowPercentage)
	{
		agentMover.SlowMovement(slowPercentage);
	}

	public int GetHit(int damage, GameObject sender)
	{
		return health.GetHit(damage);
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

	public void ParryAnimation()
	{
		agentAnimations.ParryAnimation();
	}

	public void AttackAnimation()
	{
		agentAnimations.AttackAnimation();
	}

	public float GetCurrentSpeed()
	{
		return agentMover.GetCurrentSpeed();
	}

	private void AnimateCharacter()
	{
		if (isStunned)
		{
			agentAnimations.StunAnimation();
		}
		else if (movementInput.x > 0 || movementInput.x < 0)
		{
			agentAnimations.WalkingAnimation(movementInput);
		}
		else
		{
			agentAnimations.IdleAnimation();
		}

		// Atençao!! Fazer verifica��o do salto depois da corrida uma vez que o salto tem prioridade!!
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