using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
	private new Rigidbody2D rb2d;
	private AgentAnimations agentAnimations;
	private AgentMover agentMover;
	private Health agenthealth;
	public Transform wallCheck;
	public float jumpForce, wallSlidingSpeed;

	public Vector2 wallJumoForce;

	private Vector2 lookDirection, movementInput;
	public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

	//[SerializeField]
	//public InputActionReference movement;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		agenthealth = GetComponent<Health>();
		agentAnimations = GetComponentInChildren<AgentAnimations>();
		agentMover = GetComponent<AgentMover>();
		if (agentMover == null)
		{
			Debug.LogError("AgentMover is not assigned or found in Agent.");
		}
	}

	protected void Update()
	{
		if(agenthealth.currentHealth > 0)
        {
			agentMover.MovementInput = movementInput;
			agentMover.jumpForce = jumpForce;
			agentMover.wallSlidingSpeed = wallSlidingSpeed;
			agentMover.wallJumpForce = wallJumoForce;
			agentAnimations.wallCheck = wallCheck;
			AnimateCharacter();
		}
        else
        {
			Died();
			agentAnimations.RestartLevel();
		}
		
	}

	public void GetHit(int damage, GameObject sender)
    {
		agenthealth.GetHit(damage, sender);
    }

	public void ApplyForce(Vector2 direction)
	{
		agentMover.ApplyForce(direction);
	}
	public void FaceDirection(Vector2 direction)
	{
		lookDirection = direction;
		Debug.Log(lookDirection);
		agentAnimations.LookDirection(direction);
	}

	public void PerformAttack(int damage, GameObject player = null)
	{
		//Fazer ataque
		if(player)
			player.GetComponent<Health>().GetHit(damage, gameObject);

		Debug.Log("Atacou!");
	}

	public bool CheckPlayerInFront(Vector2 direction)
	{
		bool inFront = false;
		if(lookDirection.x < 0 && direction.x < 0)
			inFront = true;

		if(lookDirection.x > 0 && direction.x > 0)
			inFront = true;
		
		return inFront;
	}

	public void ResetDash()
    {
		agentMover.ResetDash();
    }

	public void Dash(float movementInput, float dashingPower)
    {
		
		agentMover.dashingPower = dashingPower;
		agentMover.movemntInputX = movementInput;
    }

	public void Died()
	{
		
		agentMover.StopMoving();

	}

	protected virtual void AnimateCharacter()
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