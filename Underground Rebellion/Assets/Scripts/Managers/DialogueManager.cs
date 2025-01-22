using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance { get; private set; }

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

	public bool IsDialogueRunning()
	{
		return dialogueRunner.IsDialogueRunning;
	}
}
