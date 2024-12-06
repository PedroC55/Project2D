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

	private Animator animator;

	private void OnEnable()
	{
		CanvasEvent.OnUpdateHealth += UpdateHealth;
		CanvasEvent.OnWinCroissant += WinCroissant;
		CanvasEvent.OnFinishLevel += FinishLevel;
		CanvasEvent.OnGameSave += GameSaved;
	}

	private void OnDisable()
	{
		CanvasEvent.OnUpdateHealth -= UpdateHealth;
		CanvasEvent.OnWinCroissant -= WinCroissant;
		CanvasEvent.OnFinishLevel -= FinishLevel;
		CanvasEvent.OnGameSave -= GameSaved;
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
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

	private void WinCroissant(int points)
	{
		pointsText.text = $"{points}";
		animator.SetTrigger("Croissant");
	}

	private void FinishLevel()
	{
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
		SceneManager.LoadScene("End Scene");
	}
}
