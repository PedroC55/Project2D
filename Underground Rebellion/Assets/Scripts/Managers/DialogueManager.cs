using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance { get; private set; }

	[SerializeField] private YarnProject yarnProject;
	[SerializeField] private string startNode;
	[SerializeField] private List<DialogueCollider> dialogueBoxes;
	private DialogueRunner dialogueRunner;

	void Awake()
	{
		if (Instance != null)
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
		if (yarnProject != null && !SettingsManager.Instance.SkipDialogues && !string.IsNullOrEmpty(startNode))
		{
			StartNode(startNode);
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

	[YarnCommand("enable_dialogue")]
	public static void EnableDialogueBox(string dialogueNode)
	{
		List<DialogueCollider> boxes = Instance.dialogueBoxes.Where(box => box.StartNode.Equals(dialogueNode)).ToList();

		foreach(var box in boxes)
		{
			box.gameObject.SetActive(true);
		}
	}
}