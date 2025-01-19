using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance { get; private set; }

	[SerializeField] private string levelName;
	public string LevelName { get => levelName; }

	[Header("Points in Game")]
	[SerializeField] private int defeatPoints;
	[SerializeField] private int undetectedPoints;
	[SerializeField] private int aggroedPoints;
	[SerializeField] private int stunPoints;
	[SerializeField] private int secretPoints;
	[SerializeField] private int timeLimitInMinutes;
	public int TimeLimitInMinutes { get => timeLimitInMinutes; }

	[Header("Bonus at Scoreboard")]
	[SerializeField] private int defeatBonus;
	[SerializeField] private int undetectedBonus;
	[SerializeField] private int timeBonus;
	[SerializeField] private int noDeathBonus;
	public int DefeatBonus { get => defeatBonus; }
	public int UndetectedBonus { get => undetectedBonus; }
	public int TimeBonus { get => timeBonus; }
	public int NoDeathBonus { get => noDeathBonus; }


	[Header("Goals")]
	[SerializeField] private int firstGoal;
	[SerializeField] private int secondGoal;
	[SerializeField] private int thirdGoal;
	public int FirstGoal { get => firstGoal; }
	public int SecondGoal { get => secondGoal; }
	public int ThirdGoal { get => thirdGoal; }

	private Dictionary<int, SecretObject> secrets = new();
	public Dictionary<int, SecretObject> Secrets { get => secrets; }

	private Dictionary<int, EnemiesConditions> enemies = new();
	public Dictionary<int, EnemiesConditions> Enemies { get => enemies; }


	private int totalPoints = 0;
	private float timeToComplete;
	private int playerDeaths = 0;
	public int TotalPoints { get => totalPoints; }
	public float TimeToComplete { get => timeToComplete; }
	public int PlayerDeaths { get => playerDeaths; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void OnEnable()
	{
		CanvasEvent.OnUpdateScore += UpdateScore;
		CanvasEvent.OnFinishLevel += FinishLevel;
		LevelEvent.OnEnemyDied += EnemyDied;
		LevelEvent.OnRegisterEnemy += RegisterEnemy;
		LevelEvent.OnRegisterSecret += RegisterSecret;
		LevelEvent.OnPlayerDied += PlayerDied;
		LevelEvent.OnSecretFound += SecretFound;
	}

	private void OnDisable()
	{
		CanvasEvent.OnUpdateScore -= UpdateScore;
		CanvasEvent.OnFinishLevel -= FinishLevel;
		LevelEvent.OnEnemyDied -= EnemyDied;
		LevelEvent.OnRegisterEnemy -= RegisterEnemy;
		LevelEvent.OnRegisterSecret -= RegisterSecret;
		LevelEvent.OnPlayerDied -= PlayerDied;
		LevelEvent.OnSecretFound -= SecretFound;
	}

	public int GetEnemiesDefeated()
	{
		return enemies.Values.Where(x => x == EnemiesConditions.EnemyDefeated).Count();
	}

	public int GetEnemiesUndetected()
	{
		return enemies.Values.Where(x => x == EnemiesConditions.EnemyUndetected).Count();
	}

	public void DestroyScore()
	{
		Destroy(gameObject);
	}

	public void EnemyStunned()
	{
		totalPoints += stunPoints;
		CanvasEvent.UpdateScore(totalPoints);
		LevelEvent.ShowPointsInWorld(stunPoints, PointsTypes.Stun);
	}

	public void EnemyAggroed(int enemyID)
	{
		enemies[enemyID] = EnemiesConditions.EnemyAggroed;

		totalPoints += aggroedPoints;
		CanvasEvent.UpdateScore(totalPoints);
		LevelEvent.ShowPointsInWorld(aggroedPoints, PointsTypes.Aggro);
	}

	public void SaveTime(float time)
	{
		timeToComplete = time;
	}

	public void PlayerDied()
	{
		playerDeaths++;
	}

	public void SaveSecrets(List<int> secretsID)
	{
		foreach(int id in secretsID)
		{
			secrets[id].Found = true;
		}
	}

	private void UpdateScore(int score)
	{
		totalPoints = score;
	}

	private void EnemyDied(int enemyID)
	{
		enemies[enemyID] = EnemiesConditions.EnemyDefeated;
		
		totalPoints += defeatPoints;
		CanvasEvent.UpdateScore(totalPoints);
		LevelEvent.ShowPointsInWorld(defeatPoints, PointsTypes.Defeat);
	}

	private void RegisterEnemy(EnemyAI enemy)
	{
		enemies.Add(enemy.GetID(), EnemiesConditions.EnemyUndetected);
	}

	private void RegisterSecret(Secret secret)
	{
		secrets.Add(secret.ID, new SecretObject() { 
			SecretFoundSprite = secret.SecretFoundScoreboard,
			SecretNotFoundSprite = secret.SecretNotFoundScoreboard,
		});
	}

	private void FinishLevel()
	{
		int count = enemies.Values.Where(x => x == EnemiesConditions.EnemyUndetected).Count();

		totalPoints += undetectedPoints * count;
	}

	private void SecretFound(int id)
	{
		totalPoints += secretPoints;
		CanvasEvent.UpdateScore(totalPoints);
		LevelEvent.ShowPointsInWorld(secretPoints, PointsTypes.Secret);
	}
}

public class SecretObject
{
	public Sprite SecretFoundSprite;
	public Sprite SecretNotFoundSprite;

	public bool Found = false;
}

public enum EnemiesConditions { EnemyDefeated, EnemyUndetected, EnemyAggroed }
public enum PointsTypes { Defeat, Undetected, Aggro, Stun, Secret }