using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvents : MonoBehaviour
{
	public delegate void EnableDialogueBoxHandler(string dialogueNode);
	public static event EnableDialogueBoxHandler OnEnableDialogueBox;

	public static void EnableDialogueBox(string dialogueNode)
	{
		OnEnableDialogueBox?.Invoke(dialogueNode);
	}
}
