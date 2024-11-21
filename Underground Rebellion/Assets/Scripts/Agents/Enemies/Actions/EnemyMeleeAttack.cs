using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAction
{
	public string AttackAnimatorTriggerName;

	[SerializeField]
	private float buildUpDelay = 0.5f;
	[SerializeField]
	private float attackDuration = 1f;
	[SerializeField]
	private float posAttackDelay = 2f;
	//Podem ficar no script de cada ação
	[SerializeField]
	private float attackDistanceThreshold = 4f;

	private bool isAttacking = false;

	private EnemyChase enemyChase;

	protected override void Awake()
	{
		base.Awake();
		enemyChase = GetComponent<EnemyChase>();
	}

	public override void ExecuteAction(Transform player)
	{
		enemyChase.ExecuteAction(player, attackDistanceThreshold);

		isExecuting = true;
	}

	protected override void OnDisable()
	{
		base.OnDisable();

		isAttacking = false;
		enemyChase.InterruptAction();
		StopAllCoroutines();
	}

	private void Update()
	{
		if (!isExecuting)
			return;

		if (enemyChase.IsChasing())
			return;

		if (isAttacking) 
			return;

		StartCoroutine(StartAttack());
	}

	private IEnumerator StartAttack()
	{
		isAttacking = true;
		enemyAI.PerformAttack(AttackAnimatorTriggerName);
		//Fazer animação de que vai atacar
		yield return new WaitForSeconds(buildUpDelay);
		//Atacar
		yield return new WaitForSeconds(attackDuration);
		//Delay depois de atacar até a próxima ação
		yield return new WaitForSeconds(posAttackDelay);
		FinishAction();
		isAttacking = false;
	}

	//private void OnDrawGizmosSelected()
	//{
	//	Gizmos.color = Color.green;
	//	Gizmos.DrawWireSphere(transform.position, attackDistanceThreshold);
	//}
}
