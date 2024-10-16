using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrol : AgentAction
{
	[SerializeField]
	private Transform[] patrolPositions;

	private int positionsLength;
	private int currentPosition = 0;

	public override void ExecuteAction()
	{
		positionsLength = patrolPositions.Length;
		if (positionsLength <= 1)
		{
			throw new System.NullReferenceException($"Patrol do inimigo: {transform.parent.gameObject.name}, não tem posições inicializadas ou só tem 1!");
		}

		StartCoroutine(DelayAction());
	}

	private void Update()
	{
		if (!isExecuting)
			return;

		Transform movingPoint = patrolPositions[currentPosition];
		Vector2 direction = (movingPoint.position - transform.position).normalized;
		enemyAI.OnMovementInput?.Invoke(direction);

		//Quando chegar no destino final diz que a ação acabou
		float distance = Vector2.Distance(movingPoint.position, transform.position);
		if (distance <= 0.1f)
		{
			enemyAI.OnMovementInput?.Invoke(Vector2.zero);
			if(currentPosition == positionsLength - 1)
			{
				currentPosition = 0;
			}
			else
			{
				currentPosition++;
			}
			FinishAction();
		}
	}

	private void OnDrawGizmos()
	{
		foreach(Transform movingPoint in patrolPositions)
		{
			Gizmos.DrawWireSphere(movingPoint.position, 0.5f);
		}
	}
}
