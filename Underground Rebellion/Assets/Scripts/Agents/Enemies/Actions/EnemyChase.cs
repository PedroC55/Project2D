using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyChase : EnemyAction
{
	private Transform player = null;
	private float attackDistanceThreshold = Mathf.Infinity;
	private bool chaseFinished = false;
	
	private LayerMask groundLayer;
	private LayerMask detectionMask;
	[SerializeField]
	private Vector2 edgeOffset;

	private bool hasWallMovement = false;
	private WallMovement enemyWM;

	private void Start()
	{
		enemyWM = enemyAI.GetWallMovement();
		hasWallMovement = enemyWM ? true : false;
		groundLayer = LayerMask.GetMask("Ground");
		detectionMask = LayerMask.GetMask("Ground", "Player");
	}

	public override void ExecuteAction(Transform playerTransform, float attackDistance)
	{
		player = playerTransform;
		attackDistanceThreshold = attackDistance;
		isExecuting = true;
	}
	
	protected override void OnDisable()
	{
		base.OnDisable();

		chaseFinished = false;
		enemyAI.OnMovementInput?.Invoke(Vector2.zero);
	}

	private void Update()
	{
		if (enemyAI.IsGamePaused() || !isExecuting)
			return;

		float distance = Vector2.Distance(player.position, transform.position);

		if (distance <= attackDistanceThreshold)
		{
			if (ShouldKeepMoving())
			{
				enemyAI.OnMovementInput?.Invoke(GetCorrectDirection());
				return;
			}

			StopChase();
			return;
		}

		if (CheckWallOrEdge())
		{
			enemyAI.OnMovementInput?.Invoke(Vector2.zero);
			return;
		}

		enemyAI.OnMovementInput?.Invoke(GetCorrectDirection());
	}

	public bool IsChasing()
	{
		if (chaseFinished)
		{
			chaseFinished=false;
			return false;
		}

		return true;
	}

	private void StopChase()
	{
		enemyAI.OnDirectionInput?.Invoke(GetCorrectDirection());
		enemyAI.OnMovementInput?.Invoke(Vector2.zero);
		isExecuting = false;
		chaseFinished = true;
	}

	private bool ShouldKeepMoving()
	{
		if (hasWallMovement)
		{
			RaycastHit2D detectedObject = Physics2D.Raycast(transform.position, player.position - transform.position, attackDistanceThreshold, detectionMask);
			return enemyWM.IsRotating() || detectedObject.collider.gameObject.layer == 6;
		}
		return false;
	}

	private Vector2 GetCorrectDirection()
	{
		Vector2 direction = (player.position - transform.position).normalized;

		if (hasWallMovement)
		{
			enemyWM.SetDestiny(player);
			direction = enemyWM.GetDirection();
		}

		return direction;
	}

	private bool CheckWallOrEdge()
	{
		if (hasWallMovement)
			return false;

		float enemyScaleX = transform.parent.localScale.x;
		Vector2 direction = new(enemyScaleX, 0);
		if (PlayerDiferentDirection(direction))
			return false;

		// Raycast para detectar se existe superf?cie na dire??o do movimento
		RaycastHit2D hitWall = Physics2D.Raycast((Vector2)transform.position, direction, 1f, groundLayer);
		if (hitWall.collider != null)
		{
			return true;
		}

		Vector2 aux = edgeOffset;
		aux.x = edgeOffset.x * enemyScaleX;

		RaycastHit2D hitEdge = Physics2D.Raycast((Vector2)transform.position + aux, Vector2.down, 1f, groundLayer);
		if (hitEdge.collider == null)
		{
			return true;
		}

		return false;
	}

	private bool PlayerDiferentDirection(Vector2 faceDirection)
	{
		Vector2 direction = (player.position - transform.position).normalized;

		if (faceDirection.x > 0 && direction.x > 0)
			return false;
		else if (faceDirection.x < 0 && direction.x < 0)
			return false;
		else 
			return true;
	}
}
