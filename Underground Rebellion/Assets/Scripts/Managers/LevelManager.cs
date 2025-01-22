using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class LevelManager : MonoBehaviour
{
	[SerializeField]
	private Transform lastSavePosition;

	[SerializeField]
	private GameObject pointsPrefab;

	private int savedPoints = 0;

	[Header("Enemies")]
	private Dictionary<int, EnemyAI> enemiesOnLevel = new();
	private List<int> deadEnemies = new();
	private List<int> savedDeadEnemies = new();

	[Header("Secrets")]
	private Dictionary<int, Secret> secretsOnLevel = new();
	private List<int> secretsFound = new();

	private GameObject playerGO;

	private void OnEnable()
	{
		LevelEvent.OnRegisterEnemy += RegisterEnemy;
		LevelEvent.OnRegisterPlayer += RegisterPlayer;
		LevelEvent.OnRegisterSecret += RegisterSecret;
		LevelEvent.OnPlayerSave += PlayerSave;
		LevelEvent.OnPlayerDied += PlayerDied;
		LevelEvent.OnEnemyDied += EnemyDied;
		LevelEvent.OnSecretFound += SecretFound;
		LevelEvent.OnShowPointsInWorld += ShowPointsInWorld;
	}

	private void OnDisable()
	{
		LevelEvent.OnRegisterEnemy -= RegisterEnemy;
		LevelEvent.OnRegisterPlayer -= RegisterPlayer;
		LevelEvent.OnRegisterSecret -= RegisterSecret;
		LevelEvent.OnPlayerSave -= PlayerSave;
		LevelEvent.OnPlayerDied -= PlayerDied;
		LevelEvent.OnEnemyDied -= EnemyDied;
		LevelEvent.OnSecretFound -= SecretFound;
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

	private void RegisterSecret(Secret secret)
	{
		secretsOnLevel.Add(secret.ID, secret);
	}

	private void EnemyDied(int enemyID)
	{
		deadEnemies.Add(enemyID);
	}

	private void SecretFound(int secretID)
	{
		secretsFound.Add(secretID);
	}

	private void PlayerSave(Transform player)
	{
		savedPoints = ScoreManager.Instance.TotalPoints;

		lastSavePosition = player;

		CanvasEvent.GameSave();

		savedDeadEnemies.AddRange(deadEnemies);
		deadEnemies.Clear();

		//No ObjectsManager salvar o estado dos objetos e a posição dos objetos que se mexem

		ScoreManager.Instance.SaveSecrets(secretsFound);
		secretsFound.Clear();
	}

	private void PlayerDied()
	{
		CanvasEvent.UpdateScore(savedPoints);

		LevelEvent.ResetPlayer(lastSavePosition);

		deadEnemies.ForEach(id => enemiesOnLevel[id].RespawnEnemy());
		deadEnemies.Clear();

		//Reseta os objetos

		secretsFound.ForEach(id => secretsOnLevel[id].ResetSecret());
	}

	private void ShowPointsInWorld(int points, PointsTypes pointType)
	{
		Vector2 positionToInstantiate = playerGO.transform.position;
		positionToInstantiate.y += 2.5f;

		GameObject pointsGO = Instantiate(pointsPrefab, positionToInstantiate, Quaternion.identity);
		pointsGO.GetComponent<PointsInWorld>().InitScore(points, pointType);
	}
}
