using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyAction
{
	private Transform player = null;
	private float attackDistanceThreshold = Mathf.Infinity;
	private bool chaseFinished = false;
	[SerializeField]
	public LayerMask groundLayer;
	[SerializeField]
	private Vector2 edgeOffset;

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
		if (!isExecuting)
			return;

		float distance = Vector2.Distance(player.position, transform.position);

		if (!enemyAI.GetWallMovement() && CheckWallOrEdge())
		{
			enemyAI.OnMovementInput?.Invoke(Vector2.zero);
			return;
		}

		if (distance <= attackDistanceThreshold)
		{
			enemyAI.OnMovementInput?.Invoke(Vector2.zero);
			isExecuting = false;
			chaseFinished = true;
		}
		else
		{
			Vector2 direction = (player.position - transform.position).normalized;
			enemyAI.OnMovementInput?.Invoke(direction);
		}
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

	private bool CheckWallOrEdge()
	{
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

