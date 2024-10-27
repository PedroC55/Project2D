using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
	private AgentAnimations agentAnimations;
	private AgentMover agentMover;
	private Health health;

	private Vector2 lookDirection, movementInput;
	public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

	private void Awake()
	{
		agentAnimations = GetComponentInChildren<AgentAnimations>();
		agentMover = GetComponent<AgentMover>();
		health = GetComponent<Health>();
	}

	private void Update()
	{
		agentMover.MovementInput = movementInput;
		AnimateCharacter();
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

	public void Died(GameObject sender)
	{
		agentMover.StopMoving();
		agentAnimations.DeathAnimation(sender);
	}

	public void SlowMovement(int slowPercentage)
	{
		agentMover.SlowMovement(slowPercentage);
	}

	public void GetHit(int damage, GameObject sender)
	{
		health.GetHit(damage, sender);
	}

	private void AnimateCharacter()
	{
		agentAnimations.WalkingAnimation(movementInput);
	}
}