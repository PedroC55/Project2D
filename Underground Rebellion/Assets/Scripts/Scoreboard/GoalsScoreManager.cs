using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalsScoreManager : MonoBehaviour
{
	[Header("Goals Text")]
	[SerializeField] private TMP_Text firstGoalText;
	[SerializeField] private TMP_Text secondGoalText;
	[SerializeField] private TMP_Text thirdGoalText;

	[Header("Goals Images")]
	[SerializeField] private Image firstGoalImage;
	[SerializeField] private Image secondGoalImage;
	[SerializeField] private Image thirdGoalImage;

	[SerializeField] private Sprite achievedGoal;

	private int firstGoal;
	private int secondGoal;
	private int thirdGoal;

	private ScoreboardManager scoreboardManager;

	private void Start()
	{
		firstGoal = ScoreManager.Instance.GetFirstGoal();
		secondGoal = ScoreManager.Instance.GetSecondGoal();
		thirdGoal = ScoreManager.Instance.GetThirdGoal();

		firstGoalText.text = firstGoal.ToString();
		secondGoalText.text = secondGoal.ToString();
		thirdGoalText.text = thirdGoal.ToString();
	}

	public void Display(ScoreboardManager scoreboard)
	{
		scoreboardManager = scoreboard;
		StartCoroutine(ShowAchievedGoals());
	}

	private IEnumerator ShowAchievedGoals()
	{
		yield return new WaitForSeconds(0.5f);

		int totalScore = scoreboardManager.GetTotalScore();

		if (totalScore >= firstGoal)
			firstGoalImage.sprite = achievedGoal;

		yield return new WaitForSeconds(0.5f);

		if (totalScore >= secondGoal)
			secondGoalImage.sprite = achievedGoal;

		yield return new WaitForSeconds(0.5f);

		if (totalScore >= thirdGoal)
			thirdGoalImage.sprite = achievedGoal;

		scoreboardManager.GoalsDisplayFinished();
	}
}
