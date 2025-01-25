using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class CutsceneManager : MonoBehaviour
{
	public static CutsceneManager Instance { get; private set; }

	[SerializeField] private Animator canvasAnimator;
	
	private int scrollersCount = 4;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	void OnEnable()
	{
		CutsceneEvent.OnDestroyScroller += ScrollerDestroyed;	
	}

	void OnDisable()
	{
		CutsceneEvent.OnDestroyScroller -= ScrollerDestroyed;
	}

	[YarnCommand("start_fall")]
	public static void StartPlayerFall()
	{
		CutsceneEvent.StartFall();
	}

	[YarnCommand("change_scene")]
	public static void ChangeScene(string sceneName)
	{
		Instance.canvasAnimator.SetTrigger("Finish");
		Instance.StartCoroutine(Instance.ChangeLevel(sceneName));
	}

	private IEnumerator ChangeLevel(string sceneName)
	{
		yield return new WaitForSeconds(2f);
		if(sceneName.Equals("Level_1"))
			SceneManager.LoadScene("Level 1");
	}

	private void ScrollerDestroyed()
	{
		scrollersCount--;
		
		if(scrollersCount == 1)
		{
			CutsceneEvent.StopScroller();
		}
	}
}
