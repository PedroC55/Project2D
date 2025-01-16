using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemiesScoreManager : MonoBehaviour
{
    [SerializeField]
    private Sprite enemyDefeated;
	[SerializeField]
	private Sprite enemyUndetected;
	[SerializeField]
	private Sprite enemyAggroed;

	[SerializeField]
	private GameObject enemyImagePrefab;

	public void Display()
    {
		StartCoroutine(ShowEnemies());
    }

	private IEnumerator ShowEnemies()
	{
		foreach (KeyValuePair<int, EnemiesConditions> enemy in ScoreManager.Instance.Enemies)
		{
			GameObject imageObject = Instantiate(enemyImagePrefab, transform);
			Image image = imageObject.GetComponent<Image>();

			switch (enemy.Value)
			{
				case EnemiesConditions.EnemyDefeated:
					image.sprite = enemyDefeated;
					break;
				case EnemiesConditions.EnemyUndetected:
					image.sprite = enemyUndetected;
					break;
				case EnemiesConditions.EnemyAggroed:
					image.sprite = enemyAggroed;
					break;
			}
			yield return new WaitForSeconds(0.2f);
		}

		GetComponentInParent<ScoreboardManager>().EnemiesScoreFinished();
	}
}
