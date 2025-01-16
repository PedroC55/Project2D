using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField]
	private Transform lastSavePosition;

	[SerializeField]
	private GameObject pointsPrefab;

	private int savedPoints = 0;

	//Dicionario com todos os inimigos do nivel
	private Dictionary<int, EnemyAI> enemiesOnLevel = new();

	//Lista com os ids dos inimigos
	private List<int> deadEnemies = new();

	private List<int> savedDeadEnemies = new();

	private GameObject playerGO;

	private void OnEnable()
	{
		LevelEvent.OnRegisterEnemy += RegisterEnemy;
		LevelEvent.OnRegisterPlayer += RegisterPlayer;
		LevelEvent.OnPlayerSave += PlayerSave;
		LevelEvent.OnPlayerDied += PlayerDied;
		LevelEvent.OnEnemyDied += EnemyDied;
		LevelEvent.OnShowPointsInWorld += ShowPointsInWorld;
	}

	private void OnDisable()
	{
		LevelEvent.OnRegisterEnemy -= RegisterEnemy;
		LevelEvent.OnRegisterPlayer += RegisterPlayer;
		LevelEvent.OnPlayerSave -= PlayerSave;
		LevelEvent.OnPlayerDied -= PlayerDied;
		LevelEvent.OnEnemyDied -= EnemyDied;
		LevelEvent.OnShowPointsInWorld -= ShowPointsInWorld;
	}

	private void RegisterEnemy(EnemyAI enemy)
	{
		enemiesOnLevel.Add(enemy.GetID(), enemy);
	}

	private void RegisterPlayer(GameObject player)
	{
		playerGO = player;
	}

	private void EnemyDied(int enemyID)
	{
		deadEnemies.Add(enemyID);
	}

	private void PlayerSave(Transform player)
	{
		savedPoints = ScoreManager.Instance.TotalPoints;

		lastSavePosition = player;

		CanvasEvent.GameSave();
		//No EnemiesManager salvar quais deles estão vivos e mortos num dicionario
		savedDeadEnemies.AddRange(deadEnemies);
		deadEnemies.Clear();

		//No ObjectsManager salvar o estado dos objetos e a posição dos objetos que se mexem
	}

	private void PlayerDied()
	{
		CanvasEvent.UpdateScore(savedPoints);

		LevelEvent.ResetPlayer(lastSavePosition);

		//Resta os inimigos
		deadEnemies.ForEach(id => enemiesOnLevel[id].RespawnEnemy());
		deadEnemies.Clear();
		
		//Reseta os objetos
	}

	private void ShowPointsInWorld(int points, PointsTypes pointType)
	{
		Vector2 positionToInstantiate = playerGO.transform.position;
		positionToInstantiate.y += 2.5f;

		GameObject pointsGO = Instantiate(pointsPrefab, positionToInstantiate, Quaternion.identity);
		pointsGO.GetComponent<PointsInWorld>().InitScore(points, pointType);
	}
}
