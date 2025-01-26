using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DialogueCollider : MonoBehaviour
{
	[SerializeField] private string startNode;
	public string StartNode { get => startNode; }

	void Start()
	{
		if(SettingsManager.Instance.SkipDialogues)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !collision.isTrigger)
		{
			DialogueManager.Instance.StartNode(startNode);
			Destroy(gameObject);
		}
	}
}
