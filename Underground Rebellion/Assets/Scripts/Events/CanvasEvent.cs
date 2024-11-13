using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEvent : MonoBehaviour
{
	public delegate void UpdateHealthHandler(int health);
	public static event UpdateHealthHandler OnUpdateHealth;

	public delegate void WinCroissantHandler(int points);
	public static event WinCroissantHandler OnWinCroissant;

	public delegate void FinishLevelHandler();
	public static event FinishLevelHandler OnFinishLevel;

	public static void UpdateHealth(int health)
	{
		OnUpdateHealth?.Invoke(health);
	}

	public static void WinCroissant(int points)
	{
		OnWinCroissant?.Invoke(points);
	}

	public static void FinishLevel() 
	{ 
		OnFinishLevel?.Invoke();
	}
}
