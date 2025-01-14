using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour
{
	[SerializeField]
	private GoalsScoreManager goalsScoreManager;
	private EnemiesScoreManager enemiesScoreManager;
	private PointsScoreManager pointsScoreManager;

	[SerializeField]
	private TMP_Text totalScoreText;
	private int totalScore = 0;
	[SerializeField]
	private TMP_Text levelNameText;

	[SerializeField]
	private Button continueButton;

	void Start()
    {			
		enemiesScoreManager = GetComponentInChildren<EnemiesScoreManager>();
		pointsScoreManager = GetComponentInChildren<PointsScoreManager>();

		AddTotalScore(ScoreManager.Instance.GetTotalPoints());

		enemiesScoreManager.Display();
    }

	public int GetTotalScore()
	{
		return totalScore;
	}

	public void AddTotalScore(int value)
	{
		totalScore += value;
		totalScoreText.text = totalScore.ToString();
	}

	public void EnemiesScoreFinished()
	{
		continueButton.gameObject.SetActive(true);
		continueButton.onClick.AddListener(DisplayPoints);
	}

	public void PointsScoreFinished()
	{
		goalsScoreManager.Display(this);
	}

	public void GoalsDisplayFinished()
	{
		continueButton.gameObject.SetActive(true);
		continueButton.onClick.AddListener(NextScene);
	}

	private void DisplayPoints()
	{
		enemiesScoreManager.gameObject.SetActive(false);

		continueButton.onClick.RemoveAllListeners();
		continueButton.gameObject.SetActive(false);

		pointsScoreManager.gameObject.SetActive(true);
		pointsScoreManager.Display();
	}

	private void NextScene()
	{
		ScoreManager.Instance.DestroyScore();
		SceneManager.LoadScene("Main Menu");
	}
}
