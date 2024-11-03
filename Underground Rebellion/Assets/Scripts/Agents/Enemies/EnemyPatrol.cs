using System.Collections;
using UnityEngine;

public class EnemyPatrol : EnemyAction
{
	[SerializeField]
	private Transform[] patrolPositions;

	private int positionsLength;
	private int currentPosition = 0;

	[SerializeField]
	private float observationTime = 1f;
	private bool isObserving = false;

	protected override void Awake()
	{
		base.Awake();
		positionsLength = patrolPositions.Length;
		if (positionsLength <= 1)
		{
			throw new System.NullReferenceException($"Patrol do inimigo: {transform.parent.gameObject.name}, não tem posições inicializadas ou só tem 1!");
		}
	}

	protected override void OnDisable()
	{
		isExecuting = false;
		isObserving = false;
		enemyAI.OnMovementInput?.Invoke(Vector2.zero);
		StopAllCoroutines();
	}

	public override void ExecuteAction()
	{
		isExecuting = true;
	}

	private void Update()
	{
		if (!isExecuting)
			return;

		if (isObserving)
			return;

		Transform movingPoint = patrolPositions[currentPosition];
		Vector2 direction = (movingPoint.position - transform.position).normalized;
		enemyAI.OnMovementInput?.Invoke(direction);


		float distance = Vector2.Distance(movingPoint.position, transform.position);
		if (distance <= 0.1f)
		{
			enemyAI.OnMovementInput?.Invoke(Vector2.zero);
			UpdateNextPatrolPoint();
			StartCoroutine(ObsevationTime());
		}
	}

	private IEnumerator ObsevationTime()
	{
		isObserving = true;
		yield return new WaitForSeconds(observationTime);
		FinishAction();
		isObserving = false;
	}

	//Alterar isso, pq como está agora o código só vai funcionar para 2 posições, se tiver 3 ou mais fucniona,
	//porém quando chegar no ultimo nó do patrol ele vai voltar todo o caminho até o primeiro e n gradualmente.
	private void UpdateNextPatrolPoint()
	{
		currentPosition = (currentPosition == positionsLength - 1) ? 0 : currentPosition + 1;
	}


	//private void OnDrawGizmosSelected()
	//{
	//	Vector2 oldPosition = new(transform.position.x, transform.position.y);
	//	foreach(Transform movingPoint in patrolPositions)
	//	{
	//		Gizmos.color = Color.white;
	//		Gizmos.DrawWireSphere(movingPoint.position, 0.5f);
	//		Gizmos.color = Color.red;
	//		Gizmos.DrawLine(oldPosition, movingPoint.position);
	//		oldPosition = movingPoint.position;
	//	}
	//}
}
