using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAction : MonoBehaviour
{
	protected Rigidbody2D body;
	protected Animator animator;
	protected Transform player;
	protected Agent agent;
	protected EnemyAI enemyAI;

	protected bool isExecuting = false;
	[SerializeField]
	protected float buildUpTime = 1f;

	protected virtual void Awake()
	{
		body = GetComponentInParent<Rigidbody2D>();
		animator = transform.parent.gameObject.GetComponentInChildren<Animator>();
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

	public virtual void InterruptAction()
	{
		isExecuting = false;
	}

	protected void FinishAction()
	{
		Debug.Log("terminou a ação");
		isExecuting = false;
		enemyAI.ActionFinished();
	}

	protected IEnumerator DelayAction()
	{
		yield return new WaitForSeconds(buildUpTime);
		isExecuting = true;
	}
}
