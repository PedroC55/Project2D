using System.Collections;
using UnityEngine;

public class EnemyPatrol : EnemyAction
{
	[SerializeField]
	private Transform[] patrolPositions;

	private int currentPosition = 0;
	private bool reachedEnd = false;

	[SerializeField]
	private float observationTime = 1f;
	private bool isObserving = false;
	private float distanceThreshold = 0.1f;

	private WallMovement enemyWM;

	protected override void Awake()
	{
		base.Awake();
		if (patrolPositions.Length <= 1)
		{
			throw new System.NullReferenceException($"Patrol do inimigo: {transform.parent.gameObject.name}, n�o tem posi��es inicializadas ou s� tem 1!");
		}

		enemyWM = enemyAI.GetWallMovement();
		if (enemyWM)
			distanceThreshold = 0.5f;
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
		Vector2 direction;
		if (!enemyWM)
		{
			direction = (movingPoint.position - transform.position).normalized;
		}
		else
		{
			enemyWM.SetDestiny(movingPoint);
			direction = enemyWM.GetDirection();	
		}
		enemyAI.OnMovementInput?.Invoke(direction);


		float distance = Vector2.Distance(movingPoint.position, transform.position);
		if (distance <= distanceThreshold)
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

	//Alterar isso, pq como est� agora o c�digo s� vai funcionar para 2 posi��es, se tiver 3 ou mais fucniona,
	//por�m quando chegar no ultimo n� do patrol ele vai voltar todo o caminho at� o primeiro e n gradualmente.
	private void UpdateNextPatrolPoint()
	{
		if (currentPosition == patrolPositions.Length - 1)
			reachedEnd = true;

		if (currentPosition == 0)
			reachedEnd = false;

		currentPosition += reachedEnd ? -1 : 1;
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
