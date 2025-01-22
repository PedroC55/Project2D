using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private List<Image> heartsImg;
	[SerializeField]
	private TMP_Text pointsText;

	[SerializeField] 
	private TextMeshProUGUI timerText;
	private float elapsedTime;

	private Animator animator;

	private void OnEnable()
	{
		CanvasEvent.OnUpdateHealth += UpdateHealth;
		CanvasEvent.OnUpdateScore += UpdateScore;
		CanvasEvent.OnFinishLevel += FinishLevel;
		CanvasEvent.OnGameSave += GameSaved;
	}

	private void OnDisable()
	{
		CanvasEvent.OnUpdateHealth -= UpdateHealth;
		CanvasEvent.OnUpdateScore -= UpdateScore;
		CanvasEvent.OnFinishLevel -= FinishLevel;
		CanvasEvent.OnGameSave -= GameSaved;
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (DialogueManager.Instance.IsDialogueRunning())
			return;

		elapsedTime += Time.deltaTime;
		int minutes = Mathf.FloorToInt(elapsedTime / 60);
		int seconds = Mathf.FloorToInt(elapsedTime % 60);
		timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	private void UpdateHealth(int health)
	{
		for(int i = 0; i < heartsImg.Count; i++)
		{
			if(i < health)
				heartsImg[i].enabled = true;
			else
				heartsImg[i].enabled = false;
		}
	}

	private void UpdateScore(int points)
	{
		pointsText.text = $"{points}";
	}

	private void FinishLevel()
	{
		ScoreManager.Instance.SaveTime(elapsedTime);

		animator.SetTrigger("Finish");
		StartCoroutine(ChangeLevel());
	}

	private void GameSaved()
	{
		animator.SetTrigger("Save");
	}

	private IEnumerator ChangeLevel()
	{
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("Scoreboard");
	}
}
