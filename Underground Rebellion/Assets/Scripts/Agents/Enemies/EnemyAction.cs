using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
	protected Agent agent;
	protected EnemyAI enemyAI;

	protected bool isExecuting = false;
	[SerializeField]
	protected float delayTime = 0.5f;

	protected virtual void Awake()
	{
		agent = GetComponentInParent<Agent>();
		enemyAI = GetComponentInParent<EnemyAI>();
	}

	public virtual void ExecuteAction()
	{
		throw new System.NotImplementedException($"Metodo não implementado para classe: {this.GetType().FullName}, do objeto: {gameObject.name}");
	}
	public virtual void ExecuteAction(Transform player)
	{
		throw new System.NotImplementedException($"Metodo não implementado para classe: {this.GetType().FullName}, do objeto: {gameObject.name}");
	}
	public virtual void ExecuteAction(Transform player, float attackDistance)
	{
		throw new System.NotImplementedException($"Metodo não implementado para classe: {this.GetType().FullName}, do objeto: {gameObject.name}");
	}

	public virtual void InterruptAction()
	{
		this.enabled = false;
	}

	protected void FinishAction()
	{
		isExecuting = false;
		enemyAI.ActionFinished();
	}

	protected virtual IEnumerator DelayAction()
	{
		yield return new WaitForSeconds(delayTime);
	}
}
