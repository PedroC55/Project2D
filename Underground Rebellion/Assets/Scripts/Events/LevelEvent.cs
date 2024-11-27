using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvent : MonoBehaviour
{
	public delegate void RegisterEnemyHandler(EnemyAI enemyAI);
	public static event RegisterEnemyHandler OnRegisterEnemy;

	public delegate void FixElevatorHandler();
	public static event FixElevatorHandler OnFixElevator;

	public delegate void PlayerSaveHandler(Transform player);
	public static event PlayerSaveHandler OnPlayerSave;

	public delegate void PlayerDiedHandler();
	public static event PlayerDiedHandler OnPlayerDied;

	public delegate void EnemyDiedHandler(int enemyID);
	public static event EnemyDiedHandler OnEnemyDied;

	public delegate void ResetPlayerHandler(Transform lastSavePosition);
	public static event ResetPlayerHandler OnResetPlayer;

	public delegate void ResetRoomEnemiesHandler(int roomID);
	public static event ResetRoomEnemiesHandler OnResetRoomEnemies;

	public static void RegisterEnemy(EnemyAI enemyAI)
	{
		OnRegisterEnemy?.Invoke(enemyAI);
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

	public static void EnemyDied(int enemyID)
	{
		OnEnemyDied?.Invoke(enemyID);
	}

	public static void ResetPlayer(Transform lastSavePosition)
	{
		OnResetPlayer?.Invoke(lastSavePosition);
	}

	public static void ResetRoomEnemies(int roomID)
	{
		OnResetRoomEnemies?.Invoke(roomID);
	}
}
