using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//PENSAR EM UMA FORMA DE ABORTAR UMA AÇÃO QUE ESTÁ SENDO EXECUTADA QUANDO UMA CERTA CONDIÇÃO FOR ATINGIDA
//EXEMPLO A VIDA DO BOSS CHEGOU NA METADE
//Opção 1: Salvar em uma váriavel a Ação atual que está sendo executada, quando a condição acontecer chamar um metodo que para a execução daquela ação;
//Opção 2: Pensar se da pra fazer com Coroutinas;
public class EnemyAI : MonoBehaviour
{
	public UnityEvent<Vector2> OnMovementInput, OnDirectionInput;

	[SerializeField]
	private int startDirection = 1;

	[SerializeField]
	private int contactHitDamage = 1;

	private bool isActing = false;
	private bool isAggroed = false;
	private bool isDead = false;
	private bool isReseting = false;

	private Transform player;

	private Agent agent;

	#region Enemy Actions

	private EnemyPatrol patrol;
	private EnemyMeleeAttack meleeAttack;
	private EnemyAction currentAction;
	#endregion

	#region Enemy Conditions

	private EnemyLineOfSight lineOfSight;
	private EnemyEnergy enemyEnergy;
	#endregion

	private void Awake()
	{
		agent = GetComponent<Agent>();
		patrol = GetComponentInChildren<EnemyPatrol>();
		meleeAttack = GetComponentInChildren<EnemyMeleeAttack>();
		lineOfSight = GetComponentInChildren<EnemyLineOfSight>();
		
		enemyEnergy = GetComponentInChildren<EnemyEnergy>();
	}

	private void Start()
	{
		OnDirectionInput?.Invoke(new Vector2(startDirection, 0));
	}

	private void OnEnable()
	{
		HitEvent.OnHit += GetHit;
		ParryEvent.OnParry += DecreaseEnergy;
	}

	private void OnDisable()
	{
		HitEvent.OnHit -= GetHit;
		ParryEvent.OnParry -= DecreaseEnergy;
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
			if(patrol != null)
			{
				patrol.ExecuteAction();
				currentAction = patrol;
			}
		}

		isActing = true;
	}

	public void PerformAttack(string attackAnimatorTriggerName)
	{
		agent.PerformAttack(attackAnimatorTriggerName);
	}

	public bool CanAggro()
	{
		bool canAggro = true;

		if (!enemyEnergy.HasEnergy() || isReseting)
			canAggro = false;

		return canAggro;
	}

	public void Aggroed(Transform playerTransform)
	{
		if (!enemyEnergy.HasEnergy())
		{
			return;
		}

		player = playerTransform;
		if(patrol != null)
			patrol.InterruptAction();
		StartCoroutine(AggroCoroutine());
	}

	public void GetHit(int damage, GameObject sender, GameObject receiver)
	{
		if (receiver.GetInstanceID() == gameObject.GetInstanceID())
		{
			agent.GetHit(damage, sender);
		}
	}

	public void EnemyDied(GameObject sender)
	{
		currentAction.InterruptAction();
		
		OnMovementInput?.Invoke(Vector2.zero);
		
		Collider2D collider = GetComponent<Collider2D>();
		collider.enabled = false;

		isDead = true;
	}

	public void DecreaseEnergy(int amount, GameObject receiver)
	{
		if (receiver.GetInstanceID() != gameObject.GetInstanceID())
			return;

		enemyEnergy.DecreaseEnergy(amount);
	}

	public void EnemyTired()
	{
		ResetEnemy();
		isActing = false;
		OnMovementInput?.Invoke(Vector2.zero);

		//agent.StunAnimation();
	}

	public void ResetEnemy()
	{
		currentAction.InterruptAction();
		isAggroed = false;
		//isReseting = true;
		lineOfSight.ActiveLineOfSight();
	}

	public void ActionFinished()
	{
		isActing = false;
	}

	private IEnumerator AggroCoroutine()
	{
		//Mostrar animação de que o player chamou atenção do inimigo
		yield return new WaitForSeconds(1f);
		isAggroed = true;
		isActing = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("Colidiu e deu dano");
			HitEvent.GetHit(contactHitDamage, gameObject, collision.gameObject);
		}
	}
}
