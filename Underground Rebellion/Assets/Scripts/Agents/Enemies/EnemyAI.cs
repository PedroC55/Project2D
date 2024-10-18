using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//Pode ser uma classe para cada inimgo. Ex.: WormAI, BeetleAI, MoleAi, BossAi
//Onde cada um deles tem um comportamento diferente, patrol, pulo, ataque melee, ataque range,
//Mas essas a��es serem scripts separados, que s�o chamadas na AI de cada inimigo diferente
//OBS.: Talvez na classe base de a��o ja ter o metodo Update que anda o personagem at� a distancia necess�ria para realizar o ataque,
//		ao inves de em todo script ter o metodo Update. CONTUDO, tem que pensar em como mudar esse Unity Events, pq cada script vai ter essa merda querendo algum parametro pra chamar

//PENSAR EM UMA FORMA DE ABORTAR UMA A��O QUE EST� SENDO EXECUTADA QUANDO UMA CERTA CONDI��O FOR ATINGIDA
//EXEMPLO A VIDA DO BOSS CHEGOU NA METADE
//Op��o 1: Salvar em uma v�riavel a A��o atual que est� sendo executada, quando a condi��o acontecer chamar um metodo que para a execu��o daquela a��o;
//Op��o 2: Pensar se da pra fazer com Coroutinas;


//Posso mudar essa fun��o de verificar se ta na frente do player pra cada inimigo ter um objeto que � um boxcollider chamado "line of sight" para saber se aggrou ou nao
//Deixa ativado quando o player entrar no line of sight desativa
public class EnemyAI : MonoBehaviour
{
	public UnityEvent<Vector2> OnMovementInput, OnDirectionInput;
	//public UnityEvent<int, GameObject> OnAttack;

	[SerializeField]
	private int startDirection = 1;

	private bool isActing = false;
	private bool isAggroed = false;
	private bool isReseting = false;

	private Transform player;

	private EnemyPatrol patrol;
	private EnemyMeleeAttack meleeAttack;
	private EnemyLineOfSight lineOfSight;

	private void Awake()
	{
		patrol = GetComponentInChildren<EnemyPatrol>();
		meleeAttack = GetComponentInChildren<EnemyMeleeAttack>();
		lineOfSight = GetComponentInChildren<EnemyLineOfSight>();
	}

	private void Start()
	{
		OnDirectionInput?.Invoke(new Vector2(startDirection, 0));
	}

	private void Update()
	{
		if (isActing)
			return;

		if (isAggroed)
		{
			Debug.Log("Enemy will chase and attack player");
			meleeAttack.ExecuteAction(player);
		}
		else
		{
			patrol.ExecuteAction();
		}

		isActing = true;
	}

	public void Aggroed(Transform playerTransform)
	{
		player = playerTransform;
		Debug.Log("Enemy saw player and is now aggroed");
		patrol.InterruptAction();
		patrol.enabled = true;
		StartCoroutine(AggroCoroutine());
	}

	private IEnumerator AggroCoroutine()
	{
		//Mostrar anima��o de que o player chamou aten��o do inimigo
		yield return new WaitForSeconds(1f);
		isAggroed = true;
		isActing = false;
	}

	public void ResetEnemy()
	{
		meleeAttack.InterruptAction();
		isAggroed = false;
		isActing = false;
		isReseting = true;
		lineOfSight.ActiveLineOfSight();
	}

	public void ActionFinished()
	{
		isActing = false;
	}

	//void OnDrawGizmosSelected()
	//{
	//	Gizmos.DrawWireSphere(transform.position, chaseDistanceThreshold);
	//}
}
