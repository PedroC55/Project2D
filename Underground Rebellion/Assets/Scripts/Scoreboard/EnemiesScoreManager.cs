using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ScoreManager;

public class EnemiesScoreManager : MonoBehaviour
{
    [SerializeField]
    private Sprite enemyDefeated;
	[SerializeField]
	private Sprite enemyUndetected;
	[SerializeField]
	private Sprite enemyDetected;

	[SerializeField]
	private GameObject enemyImagePrefab;

	public void Display()
    {
		StartCoroutine(ShowEnemies());
    }

	private IEnumerator ShowEnemies()
	{
		//foreach (KeyValuePair<int, EnemiesConditions> enemy in scoreManager.GetEnemies())
		//{
		//	GameObject imageObject = Instantiate(enemyImagePrefab, transform);
		//	Image image = imageObject.GetComponent<Image>();

		//	switch (enemy.Value)
		//	{
		//		case EnemiesConditions.EnemeyDefeated:
		//			image.sprite = enemyDefeated;
		//			break;
		//		case EnemiesConditions.EnemyUndetected:
		//			image.sprite = enemyUndetected;
		//			break;
		//		case EnemiesConditions.EnemyDetected:
		//			image.sprite = enemyDetected;
		//			break;
		//	}
		//	yield return new WaitForSeconds(0.2f);
		//}

		foreach (EnemiesConditions enemy in ScoreManager.Instance.GetEnemies())
		{
			GameObject imageObject = Instantiate(enemyImagePrefab, transform);
			Image image = imageObject.GetComponent<Image>();

			switch (enemy)
			{
				case EnemiesConditions.EnemyDefeated:
					image.sprite = enemyDefeated;
					break;
				case EnemiesConditions.EnemyUndetected:
					image.sprite = enemyUndetected;
					break;
				case EnemiesConditions.EnemyDetected:
					image.sprite = enemyDetected;
					break;
			}
			yield return new WaitForSeconds(0.1f);
		}

		GetComponentInParent<ScoreboardManager>().EnemiesScoreFinished();
	}
}
