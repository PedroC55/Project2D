using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class CutsceneView : MonoBehaviour
{
	[Serializable]
	public class CutsceneData
	{
		public int cutsceneID;
		public Sprite displayCutscene;
	}

	[SerializeField] private List<CutsceneData> cutsceneData;
	[SerializeField] private Image cutsceneImage;
	private Dictionary<int, Sprite> cutsceneDictionary = new();

	private Animator canvasAnimator;

	void Start()
	{
		canvasAnimator = GetComponentInParent<Animator>();

		foreach (var cutscene in cutsceneData)
		{
			cutsceneDictionary.Add(cutscene.cutsceneID, cutscene.displayCutscene);
		}
	}

	[YarnCommand("show_cutscene")]
	public void ShowCutscene(int cutsceneID)
	{
		cutsceneImage.sprite = cutsceneDictionary[cutsceneID];
		canvasAnimator.SetTrigger("ShowCutscene");
	}

	[YarnCommand("next_cutscene")]
	public void NextCutscene()
	{
		canvasAnimator.SetTrigger("NextCutscene");
	}

	[YarnCommand("hide_cutscene")]
	public void HideCutscene()
	{
		canvasAnimator.SetTrigger("HideCutscene");
	}
}
