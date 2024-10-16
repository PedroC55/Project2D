using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAction
{
	//Ficar no script da ação
	[SerializeField]
	private int damage = 1;

	//Criar um script generico para ataques dos inimigos, mellee, shot, jump (exemplos), se existem 2 ataques que são melee teoricamente não precisa de 2 scripts diferentes
	[SerializeField]
	private float attackDelay = 1f;
	//Tempo antes de realizar a ação
	[SerializeField]
	private float buildUpDelay = 0.5f;


	//Podem ficar no script de cada ação
	[SerializeField]
	private float chaseDistanceThreshold = 15f, attackDistanceThreshold = 4f;

	//private void Update()
	//{
	//	float distance = Vector2.Distance(player.position, transform.position);
	//	if (distance < chaseDistanceThreshold)
	//	{
	//		if (distance <= attackDistanceThreshold)
	//		{
	//			OnMovementInput?.Invoke(Vector2.zero);

	//			if (canAttack)
	//			{
	//				OnAttack?.Invoke(damage, null);
	//				canAttack = false;
	//				StartCoroutine(DelayAttack());
	//			}
	//		}
	//		else
	//		{
	//			//chasing player
	//			Vector2 direction = (player.position - transform.position).normalized;
	//			if (enemyAgent.CheckPlayerInFront(direction))
	//				aggroed = true;

	//			if (aggroed)
	//				OnMovementInput?.Invoke(direction);
	//		}
	//	}
	//}

	//private IEnumerator DelayAttack()
	//{
	//	yield return new WaitForSeconds(attackDelay);
	//	canAttack = true;
	//}

	//private IEnumerator AttackCooldown()
	//{
	//	yield return new WaitForSeconds(attackDelay);
	//	canAttack = true;
	//}
}
