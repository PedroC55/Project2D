using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	private int croissants = 0;
	private int savedCroissants = 0;

	[SerializeField]
	private Transform lastSavePosition;

	//Dicionario com todos os inimigos do nivel
	private Dictionary<int, EnemyAI> enemiesOnLevel = new();

	//Lista com os ids dos inimigos
	private List<int> deadEnemies = new();

	private List<int> savedDeadEnemies = new();

	private void OnEnable()
	{
		LevelEvent.OnRegisterEnemy += RegisterEnemy;
		LevelEvent.OnPlayerSave += PlayerSave;
		LevelEvent.OnPlayerDied += PlayerDied;
		LevelEvent.OnEnemyDied += EnemyDied;
	}

	private void OnDisable()
	{
		LevelEvent.OnRegisterEnemy -= RegisterEnemy;
		LevelEvent.OnPlayerSave -= PlayerSave;
		LevelEvent.OnPlayerDied -= PlayerDied;
		LevelEvent.OnEnemyDied -= EnemyDied;
	}

	private void RegisterEnemy(EnemyAI enemy)
	{
		enemiesOnLevel.Add(enemy.GetID(), enemy);
	}

	private void EnemyDied(int enemyID)
	{
		deadEnemies.Add(enemyID);
		WinCroissant();
	}

	private void PlayerSave(Transform player)
	{
		savedCroissants = croissants;

		lastSavePosition = player;

		CanvasEvent.GameSave();
		//No EnemiesManager salvar quais deles est�o vivos e mortos num dicionario
		savedDeadEnemies.AddRange(deadEnemies);
		deadEnemies.Clear();

		//No ObjectsManager salvar o estado dos objetos e a posi��o dos objetos que se mexem
	}

	private void PlayerDied()
	{
		croissants = savedCroissants;

		LevelEvent.ResetPlayer(lastSavePosition);

		//Resta os inimigos
		deadEnemies.ForEach(id => enemiesOnLevel[id].RespawnEnemy());
		deadEnemies.Clear();
		
		//Reseta os objetos
	}
	private void WinCroissant()
	{
		croissants++;
		CanvasEvent.WinCroissant(croissants);
	}
}
