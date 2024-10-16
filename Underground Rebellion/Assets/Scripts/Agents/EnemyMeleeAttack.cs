using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : AgentAction
{
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
