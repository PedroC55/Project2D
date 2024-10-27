using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyAction
{
	private Transform player = null;
	private float attackDistanceThreshold = Mathf.Infinity;
	private bool chaseFinished = false;

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
		if (distance <= attackDistanceThreshold)
		{
			enemyAI.OnMovementInput?.Invoke(Vector2.zero);
			isExecuting = false;
			chaseFinished = true;
		}
		else
		{
			Vector2 direction = (player.position - transform.position).normalized;

			//Depois melhorar o sistema de Chase para não bater nas paredes, não cair das plataformas e futuramente dar a volta no mapa ou algo do tipo
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
}

