using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAI : EnemyAI
{
	#region Enemy Actions

	private EnemyPatrol patrol;
	//private EnemyMeleeAttack meleeAttack;
	private EnemyRangedAttack rangedAttack;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		wallMovement = GetComponent<WallMovement>();
		patrol = GetComponentInChildren<EnemyPatrol>();
		rangedAttack = GetComponentInChildren<EnemyRangedAttack>();
		//meleeAttack = GetComponentInChildren<EnemyMeleeAttack>();
	}

	private void Update()
	{
		if (isActing || isDead || !enemyEnergy.HasEnergy())
			return;

		if (isAggroed)
		{
			rangedAttack.ExecuteAction(player);
			currentAction = rangedAttack;
			//meleeAttack.ExecuteAction(player);
			//currentAction = meleeAttack;
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
