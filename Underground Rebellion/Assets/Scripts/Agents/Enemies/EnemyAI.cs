using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
	[SerializeField]
	private int id;

	public UnityEvent<Vector2> OnMovementInput, OnDirectionInput;
	protected Transform player;
	protected Agent agent;

	[SerializeField]
	private int startDirection = 1;
	private Vector2 initialPosition;

	[SerializeField]
	private int contactHitDamage = 1;

	protected bool isActing = false;
	protected bool isAggroed = false;
	protected bool isDead = false;

	public int roomID;

	#region Enemy Actions
	protected EnemyAction currentAction;
	#endregion

	#region Enemy Conditions

	protected EnemyLineOfSight lineOfSight;
	protected EnemyEnergy enemyEnergy;
	protected WallMovement wallMovement;
	#endregion

	protected virtual void Awake()
	{
		agent = GetComponent<Agent>();
		
		lineOfSight = GetComponentInChildren<EnemyLineOfSight>();
		enemyEnergy = GetComponentInChildren<EnemyEnergy>();
		initialPosition = gameObject.transform.position;
	}

	private void Start()
	{
		LevelEvent.RegisterEnemy(this);

		OnDirectionInput?.Invoke(new Vector2(startDirection, 0));
	}

	private void OnEnable()
	{
		HitEvent.OnHit += GetHit;
		ParryEvent.OnParry += DecreaseEnergy;
		LevelEvent.OnResetRoomEnemies += ResetEnemy;
	}

	private void OnDisable()
	{
		HitEvent.OnHit -= GetHit;
		ParryEvent.OnParry -= DecreaseEnergy;
		LevelEvent.OnResetRoomEnemies -= ResetEnemy;
	}

	public int GetID()
	{
		return id;
	}

	public WallMovement GetWallMovement()
	{
		return GetComponent<WallMovement>();
	}

	public void SetRoomID(int id)
	{
		roomID = id;
	}

	public bool CanAggro()
	{
		if (isAggroed || !enemyEnergy.HasEnergy() || (wallMovement && wallMovement.IsRotating()))
		{
			return false;
		}

		return true;
	}
	public void PerformAttack(string attackAnimatorTriggerName)
	{
		agent.PerformAttack(attackAnimatorTriggerName);
	}

	public void Aggroed(Transform playerTransform)
	{
		if (!enemyEnergy.HasEnergy())
		{
			return;
		}

		player = playerTransform;
		isAggroed = true;
		
		if(currentAction != null)
			currentAction.InterruptAction();

		StartCoroutine(AggroCoroutine());
	}

	public void GetHit(int damage, GameObject sender, GameObject receiver)
	{
		if (sender.CompareTag("Player") && receiver.GetInstanceID() == gameObject.GetInstanceID() && !enemyEnergy.HasEnergy())
		{
			agent.GetHit(damage, sender);
		}
		if (sender.CompareTag("Player") && receiver.GetInstanceID() == gameObject.GetInstanceID() && enemyEnergy.HasEnergy())
        {
			SoundManager.PlaySound(SoundType.HIT_DENIED);
        }
    }

	public bool IsDead()
	{
		return isDead;
	}

	public void EnemyDied()
	{
		//Ganha pontos por matar os inimigos, mas o ideal depois é ganhar ponto por limpar a sala
		LevelEvent.EnemyDied(id);
		
		isDead = true;
		currentAction.InterruptAction();
		
		Collider2D collider = GetComponent<Collider2D>();
		collider.isTrigger = true;
		OnMovementInput?.Invoke(Vector2.zero);

		agent.Died();

		StartCoroutine(DeathCoroutine());
	}


	public void DecreaseEnergy(int amount, GameObject receiver)
	{
		if (receiver.GetInstanceID() != gameObject.GetInstanceID())
			return;

		enemyEnergy.DecreaseEnergy(amount);
	}

	public void EnemyTired()
	{
		LostAggro();
		isActing = false;
		OnMovementInput?.Invoke(Vector2.zero);

		//agent.StunAnimation();
	}

	public void ActionFinished()
	{
		isActing = false;
	}

	public void LostAggro()
	{
		currentAction.InterruptAction();
		isAggroed = false;
		lineOfSight.ResetLineOfSight();
	}

	public void RespawnEnemy()
	{
		gameObject.SetActive(true);

		agent.ResetAgent(false);

		isDead = false;

		Collider2D collider = GetComponent<Collider2D>();
		collider.isTrigger = false;

		ResetEnemy(roomID);
	}

	private void ResetEnemy(int id)
	{
		if(roomID == id)
		{
			if (isAggroed)
			{
				LostAggro();
			}
			else
			{
				currentAction.InterruptAction();
				isActing = false;
			}

			OnDirectionInput?.Invoke(new Vector2(startDirection, 0));
			gameObject.transform.position = initialPosition;
			enemyEnergy.ResetEnergy();
		}
	}

	private IEnumerator AggroCoroutine()
	{
		yield return new WaitForSeconds(1f);
		isAggroed = true;
		isActing = false;
	}

	private IEnumerator DeathCoroutine()
	{
		//Mostrar animação de que o player chamou atenção do inimigo
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && !isDead)
		{
			Debug.Log("Colidiu e deu dano");
			HitEvent.GetHit(contactHitDamage, gameObject, collision.gameObject);
		}
	}
}
