using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEvent : MonoBehaviour
{
	public delegate void UpdateHealthHandler(int health);
	public static event UpdateHealthHandler OnUpdateHealth;

	public delegate void UpdateScoreHandler(int points);
	public static event UpdateScoreHandler OnUpdateScore;

	public delegate void FinishLevelHandler();
	public static event FinishLevelHandler OnFinishLevel;

	public delegate void GameSaveHandler();
	public static event GameSaveHandler OnGameSave;

	public static void UpdateHealth(int health)
	{
		OnUpdateHealth?.Invoke(health);
	}

	public static void UpdateScore(int points)
	{
		OnUpdateScore?.Invoke(points);
	}

	public static void FinishLevel() 
	{ 
		OnFinishLevel?.Invoke();
	}

	public static void GameSave()
	{
		OnGameSave?.Invoke();
	}
}
