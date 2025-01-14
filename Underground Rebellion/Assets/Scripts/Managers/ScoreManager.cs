using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance { get; private set; }

	//private Dictionary<int, EnemiesConditions> enemies = new();

	[Header("Points in Game")]
	[SerializeField] private int defeatPoints;
	[SerializeField] private int undetectedPoints;
	[SerializeField] private int timeLimitInMinutes;

	[Header("Bonus at Scoreboard")]
	[SerializeField] private int defeatBonus;
	[SerializeField] private int undetectedBonus;
	[SerializeField] private int timeBonus;
	[SerializeField] private int noDeathBonus;

	[Header("Goals")]
	[SerializeField] private int firstGoal;
	[SerializeField] private int secondGoal;
	[SerializeField] private int thirdGoal;

	[SerializeField] private List<EnemiesConditions> enemies = new();

	[SerializeField] private string levelName;
	[SerializeField] private int totalPoints;
	[SerializeField] private float timeToComplete;
	[SerializeField] private int playerDeaths;
	/*Score Manager da cena anterior
	 * Quantos e quais secrets achou
     */
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

	//public Dictionary<int, EnemiesConditions> GetEnemies()
	//{
	//    return enemies;
	//}

	public List<EnemiesConditions> GetEnemies()
	{
		return enemies;
	}

	public int GetEnemiesDefeated()
	{
		return enemies.Where(x => x == EnemiesConditions.EnemyDefeated).Count();
	}

	public int GetEnemiesUndetected()
	{
		return enemies.Where(x => x == EnemiesConditions.EnemyUndetected).Count();
	}

	public int GetTotalPoints()
	{
		return totalPoints;
	}

	public float GetTimeToComplete()
	{
		return timeToComplete;
	}

	public int GetPlayerDeaths()
	{
		return playerDeaths;
	}

	public int GetDefeatBonus()
	{
		return defeatBonus;
	}

	public int GetUndetectedBonus()
	{
		return undetectedBonus;
	}

	public int GetTimeLimitInMinutes()
	{
		return timeLimitInMinutes;
	}

	public int GetTimeBonus()
	{
		return timeBonus;
	}

	public int GetNoDeathBonus()
	{
		return noDeathBonus;
	}

	public int GetFirstGoal()
	{
		return firstGoal;
	}

	public int GetSecondGoal()
	{
		return secondGoal;
	}

	public int GetThirdGoal()
	{
		return thirdGoal;
	}

	public void DestroyScore()
	{
		Destroy(gameObject);
	}
}

public enum EnemiesConditions { EnemyDefeated, EnemyUndetected, EnemyDetected }