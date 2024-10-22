using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
	private AgentAnimations agentAnimations;
	private AgentMover agentMover;

	private Vector2 lookDirection, movementInput;
	public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

	private void Awake()
	{
		agentAnimations = GetComponentInChildren<AgentAnimations>();
		agentMover = GetComponent<AgentMover>();
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

	public void Died(GameObject sender)
	{
		agentMover.StopMoving();
		agentAnimations.DeathAnimation(sender);
	}

	protected virtual void AnimateCharacter()
	{
		agentAnimations.WalkingAnimation(movementInput);
	}
}