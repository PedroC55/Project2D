using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAI : EnemyAI
{
	#region Enemy Actions

	private EnemyPatrol patrol;
	private EnemyMeleeAttack meleeAttack;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		patrol = GetComponentInChildren<EnemyPatrol>();
		meleeAttack = GetComponentInChildren<EnemyMeleeAttack>();
	}

	private void Update()
	{
		if (isActing || isDead || !enemyEnergy.HasEnergy())
			return;

		if (isAggroed)
		{
			meleeAttack.ExecuteAction(player);
			currentAction = meleeAttack;
		}
		else
		{
			if (patrol != null)
			{
				patrol.ExecuteAction();
				currentAction = patrol;
			}
		}

		isActing = true;
	}
}
