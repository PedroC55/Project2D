using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
	protected EnemyAI enemyAI;

	protected bool isExecuting = false;
	[SerializeField]
	protected float delayTime = 0.5f;

	protected virtual void Awake()
	{
		enemyAI = GetComponentInParent<EnemyAI>();
	}

	protected virtual void OnDisable()
	{
		FinishAction();
	}

	public virtual void ExecuteAction()
	{
		throw new System.NotImplementedException($"Metodo n�o implementado para classe: {this.GetType().FullName}, do objeto: {transform.parent.gameObject.name}");
	}
	public virtual void ExecuteAction(Transform player)
	{
		throw new System.NotImplementedException($"Metodo n�o implementado para classe: {this.GetType().FullName}, do objeto: {transform.parent.gameObject.name}");
	}
	public virtual void ExecuteAction(Transform player, float attackDistance)
	{
		throw new System.NotImplementedException($"Metodo n�o implementado para classe: {this.GetType().FullName}, do objeto: {transform.parent.gameObject.name}");
	}

	public EnemyAI GetEnemyAI()
	{
		return enemyAI;
	}

	public virtual void InterruptAction()
	{
		this.enabled = false;
		this.enabled = true;
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
