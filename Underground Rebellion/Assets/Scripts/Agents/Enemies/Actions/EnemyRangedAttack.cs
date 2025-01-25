using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : EnemyAction
{
	public string AttackAnimatorTriggerName;

	[SerializeField]
	private float buildUpDelay = 1f;
	[SerializeField]
	private float attackDuration = 0.5f;
	[SerializeField]
	private float posAttackDelay = 2f;
	[SerializeField]
	private float attackDistanceThreshold = 10f;

	[SerializeField]
	private GameObject projectilePrefab;
	[SerializeField]
	private Transform shootPoint;
	[SerializeField]
	private float projectileForce = 10f;

	private bool isAttacking = false;

	private EnemyChase enemyChase;
	private Transform player;

	protected override void Awake()
	{
		base.Awake();
		enemyChase = GetComponent<EnemyChase>();
	}

	public override void ExecuteAction(Transform playerTransform)
	{
		player = playerTransform;
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
		if (enemyAI.IsGamePaused() || !isExecuting || enemyChase.IsChasing() || isAttacking)
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
		ThrowProjectile();
		yield return new WaitForSeconds(attackDuration);
		//Delay depois de atacar até a próxima ação
		yield return new WaitForSeconds(posAttackDelay);
		FinishAction();
		isAttacking = false;
	}

	private void ThrowProjectile()
	{
		GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, transform.parent.rotation);

		projectile.GetComponent<Projectile>().SetShotter(enemyAI);
		projectile.GetComponent<Projectile>().SetForce(projectileForce, player);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, attackDistanceThreshold);
	}
}
