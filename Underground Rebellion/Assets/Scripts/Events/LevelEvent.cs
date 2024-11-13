using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvent : MonoBehaviour
{
	public delegate void WinCroissantHandler();
	public static event WinCroissantHandler OnWinCroissant;

	public delegate void FixElevatorHandler();
	public static event FixElevatorHandler OnFixElevator;

	public static void WinCroissant()
	{
		OnWinCroissant?.Invoke();
	}

	public static void FixElevator()
	{
		OnFixElevator?.Invoke();
	}
}
