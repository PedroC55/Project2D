using UnityEngine;
using UnityEngine.Events;

//Pode ser uma classe para cada inimgo. Ex.: WormAI, BeetleAI, MoleAi, BossAi
//Onde cada um deles tem um comportamento diferente, patrol, pulo, ataque melee, ataque range,
//Mas essas ações serem scripts separados, que são chamadas na AI de cada inimigo diferente
//OBS.: Talvez na classe base de ação ja ter o metodo Update que anda o personagem até a distancia necessária para realizar o ataque,
//		ao inves de em todo script ter o metodo Update. CONTUDO, tem que pensar em como mudar esse Unity Events, pq cada script vai ter essa merda querendo algum parametro pra chamar

//PENSAR EM UMA FORMA DE ABORTAR UMA AÇÃO QUE ESTÁ SENDO EXECUTADA QUANDO UMA CERTA CONDIÇÃO FOR ATINGIDA
//EXEMPLO A VIDA DO BOSS CHEGOU NA METADE
//Opção 1: Salvar em uma váriavel a Ação atual que está sendo executada, quando a condição acontecer chamar um metodo que para a execução daquela ação;
//Opção 2: Pensar se da pra fazer com Coroutinas;


//Posso mudar essa função de verificar se ta na frente do player pra cada inimigo ter um objeto que é um boxcollider chamado "line of sight" para saber se aggrou ou nao
//Deixa ativado quando o player entrar no line of sight desativa
public class EnemyAI : MonoBehaviour
{
	public UnityEvent<Vector2> OnMovementInput, OnDirectionInput;
	//public UnityEvent<int, GameObject> OnAttack;

	[SerializeField]
	private Transform player;

	[SerializeField]
	private int startDirection = 1;

	//Podem ficar no script de cada ação
	[SerializeField]
	private float chaseDistanceThreshold = 15f, attackDistanceThreshold = 4f;

	//Ficar no script da ação
	[SerializeField]
	private int damage = 1;

	//Criar um script generico para ataques dos inimigos, mellee, shot, jump (exemplos), se existem 2 ataques que são melee teoricamente não precisa de 2 scripts diferentes
	[SerializeField]
	private float attackDelay = 1f;
	//Tempo antes de realizar a ação
	[SerializeField]
	private float buildUpDelay = 0.5f;
	//Variavel que vai dizer se o inimigo terminou de realizar a ação random dele e pode escolher outra ação
	private bool isActing = false;
	private bool aggroed = false;

	//Mudar para boxcollider
	private Agent enemyAgent;
	private EnemyPatrol patrol;
	private EnemyMeleeAttack meleeAttack;

	private void Awake()
	{
		enemyAgent = GetComponent<Agent>();
		patrol = GetComponentInChildren<EnemyPatrol>();

	}

	private void Start()
	{
		OnDirectionInput?.Invoke(new Vector2(startDirection, 0));
	}

	private void Update()
	{
		if (isActing)
			return;

		//Aqui vai mandar fazer patrol caso não esteja agrado e tenha como fazer patrol
		//Caso esteja agrado vai perseguir o player por uma distancia maxima e chegar perto para atacar

		patrol.ExecuteAction();
		//if (aggroed)
		//{
		//	aggroed = true;
		//	meleeAttack.ExecuteAction();
		//}
		//else
		//{
		//	patrol.ExecuteAction();
		//}

		isActing = true;
	}

	public void Aggroed()
	{
		aggroed = true;
		patrol.InterruptAction();
	}

	public void ActionFinished()
	{
		isActing = false;
	}

	//private void OnTriggerEnter2D(Collider2D collision)
	//{
	//	if (collision.gameObject.CompareTag("Player"))
	//	{
	//		//EnemyAI.Aggroed();
	//	}
	//}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, chaseDistanceThreshold);
	}
}
