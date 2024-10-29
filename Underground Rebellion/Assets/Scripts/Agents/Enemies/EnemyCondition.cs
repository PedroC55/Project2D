using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCondition : MonoBehaviour
{
	protected EnemyAI enemyAI;

	protected virtual void Awake()
	{
		enemyAI = GetComponentInParent<EnemyAI>();
	}

	protected virtual void ConditionRechead()
	{
		throw new System.NotImplementedException($"Metodo não implementado para classe: {this.GetType().FullName}, do objeto: {transform.parent.gameObject.name}");
	}
}
