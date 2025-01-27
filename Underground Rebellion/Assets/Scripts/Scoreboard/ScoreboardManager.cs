using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour
{
	[SerializeField]
	private GoalsScoreboard goalsScoreboard;
	private EnemiesScoreboard enemiesScoreboard;
	private PointsScoreboard pointsScoreboard;

	[SerializeField]
	private TMP_Text totalScoreText;
	private int totalScore = 0;
	[SerializeField]
	private TMP_Text levelNameText;

	[SerializeField]
	private Button continueButton;

	void Start()
    {
		enemiesScoreboard = GetComponentInChildren<EnemiesScoreboard>();
		pointsScoreboard = GetComponentInChildren<PointsScoreboard>();

		levelNameText.text = ScoreManager.Instance.LevelName;
		AddTotalScore(ScoreManager.Instance.TotalPoints);

		enemiesScoreboard.Display();
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
		goalsScoreboard.Display(this);
	}

	public void GoalsDisplayFinished()
	{
		continueButton.gameObject.SetActive(true);
		continueButton.onClick.AddListener(NextScene);
	}

	private void DisplayPoints()
	{
		enemiesScoreboard.gameObject.SetActive(false);

		continueButton.onClick.RemoveAllListeners();
		continueButton.gameObject.SetActive(false);

		pointsScoreboard.gameObject.SetActive(true);
		pointsScoreboard.Display();
	}

	private void NextScene()
	{
		ScoreManager.Instance.DestroyScore();
		SceneManager.LoadScene("End Scene");
	}
}
