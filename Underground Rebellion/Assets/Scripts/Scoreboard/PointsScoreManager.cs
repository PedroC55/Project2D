using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PointsScoreManager : MonoBehaviour
{
	[Header("Level Complete")]
	[SerializeField] private TMP_Text completeScore;

	[Header("Defeated Bonus")]
	[SerializeField] private TMP_Text defeatedCountText;
	[SerializeField] private TMP_Text defeatedScoreText;
	[SerializeField] private TMP_Text defeatedBonusText;

	[Header("Undetected Bonus")]
	[SerializeField] private TMP_Text undetectedCountText;
	[SerializeField] private TMP_Text undetectedScoreText;
	[SerializeField] private TMP_Text undetectedBonusText;

	[Header("Under 15 Minutes")]
	[SerializeField] private TMP_Text timeCountText;
	[SerializeField] private TMP_Text timeScoreText;
	[SerializeField] private TMP_Text timeBonusText;
	[SerializeField] private GameObject timeRedLine;

	[Header("No Deaths")]
	[SerializeField] private TMP_Text deathCountText;
	[SerializeField] private TMP_Text deathScoreText;
	[SerializeField] private TMP_Text deathBonusText;
	[SerializeField] private GameObject deathRedLine;

	[Header("Text Colors")]
	[SerializeField] private Color achievedColor;
	[SerializeField] private Color notAchievedColor;

	private ScoreboardManager scoreboardManager;

	private int defeatedScore;
	private int undetectedScore;
	private bool timeAchieved = false;
	private bool deathAchieved = false;

	void Start()
	{
		scoreboardManager = GetComponentInParent<ScoreboardManager>();
		UpdatePoints();
		gameObject.SetActive(false);
	}

	public void Display()
	{
		StartCoroutine(ShowPoints());
	}

	private void UpdatePoints()
	{
		completeScore.text = ScoreManager.Instance.GetTotalPoints().ToString();

		int count = ScoreManager.Instance.GetEnemiesDefeated();
		int bonus = ScoreManager.Instance.GetDefeatBonus();
		defeatedScore = count * bonus;
		timeBonusText.text = $"X {bonus.ToString()}";
		defeatedCountText.text = count.ToString();

		count = ScoreManager.Instance.GetEnemiesUndetected();
		bonus = ScoreManager.Instance.GetUndetectedBonus();
		undetectedScore = count * bonus;
		undetectedBonusText.text = $"X {bonus.ToString()}";
		undetectedCountText.text = count.ToString();
		
		float time = ScoreManager.Instance.GetTimeToComplete();
		UpdateTimeCount(time);
		timeAchieved = (time <= ScoreManager.Instance.GetTimeLimitInMinutes() * 60);
		timeBonusText.text = ScoreManager.Instance.GetTimeBonus().ToString();
		timeBonusText.color = timeAchieved ? achievedColor : notAchievedColor;

		count = ScoreManager.Instance.GetPlayerDeaths();
		deathCountText.text = count.ToString();
		deathAchieved = (count == 0);
		deathBonusText.text = ScoreManager.Instance.GetNoDeathBonus().ToString();
		deathBonusText.color = deathAchieved ? achievedColor : notAchievedColor;
	}

	private void UpdateTimeCount(float time)
	{
		int minutes = Mathf.FloorToInt(time / 60);
		int seconds = Mathf.FloorToInt(time % 60);
		timeCountText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	private IEnumerator ShowPoints()
	{
		yield return new WaitForSeconds(1f);

		defeatedScoreText.text = defeatedScore.ToString();
		scoreboardManager.AddTotalScore(defeatedScore);

		yield return new WaitForSeconds(0.5f);

		undetectedScoreText.text = undetectedScore.ToString();
		scoreboardManager.AddTotalScore(undetectedScore);

		yield return new WaitForSeconds(0.5f);

		if (timeAchieved)
		{
			int bonus = ScoreManager.Instance.GetTimeBonus();
			timeScoreText.text = bonus.ToString();
			scoreboardManager.AddTotalScore(bonus);
		}
		else
		{
			timeScoreText.text = "0";
			timeRedLine.SetActive(true);
		}

		yield return new WaitForSeconds(0.5f);

		if (deathAchieved)
		{
			int bonus = ScoreManager.Instance.GetNoDeathBonus();
			deathScoreText.text = bonus.ToString();
			scoreboardManager.AddTotalScore(bonus);
		}
		else
		{
			deathScoreText.text = "0";
			deathRedLine.SetActive(true);
		}

		scoreboardManager.PointsScoreFinished();
	}
}