using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvent : MonoBehaviour
{
	public delegate void WinCroissantHandler();
	public static event WinCroissantHandler OnWinCroissant;

	public delegate void FixElevatorHandler();
	public static event FixElevatorHandler OnFixElevator;

	public delegate void PlayerSaveHandler(Transform player);
	public static event PlayerSaveHandler OnPlayerSave;

	public delegate void PlayerDiedHandler();
	public static event PlayerDiedHandler OnPlayerDied;

	public delegate void ResetPlayerHandler(Transform lastSavePosition);
	public static event ResetPlayerHandler OnResetPlayer;

	public static void WinCroissant()
	{
		OnWinCroissant?.Invoke();
	}

	public static void FixElevator()
	{
		OnFixElevator?.Invoke();
	}

	public static void PlayerSave(Transform player)
	{
		OnPlayerSave?.Invoke(player);
	}

	public static void PlayerDied()
	{
		OnPlayerDied?.Invoke();
	}

	public static void ResetPlayer(Transform lastSavePosition)
	{
		OnResetPlayer?.Invoke(lastSavePosition);
	}
}
