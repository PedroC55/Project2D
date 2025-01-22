using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance { get; private set; }

	[SerializeField] private YarnProject yarnProject;
	private DialogueRunner dialogueRunner;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			dialogueRunner = GetComponent<DialogueRunner>();
			Instance = this;
		}
	}

	private void Start()
	{
		if (yarnProject != null && !SettingsManager.Instance.SkipDialogues)
		{
			StartNode("Start");
		}
	}

	public bool IsDialogueRunning()
	{
		return dialogueRunner.IsDialogueRunning;
	}

	public void StartNode(string nodeName)
	{
		dialogueRunner.StartDialogue(nodeName);
	}
}
