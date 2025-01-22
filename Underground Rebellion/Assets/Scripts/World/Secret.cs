using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secret : MonoBehaviour
{
    [SerializeField] private int id;
	[SerializeField] private Sprite secretFoudScoreboard;
	[SerializeField] private Sprite secretNotFoudScoreboard;
	public Sprite SecretFoundScoreboard { get => secretFoudScoreboard; }
	public Sprite SecretNotFoundScoreboard { get => secretNotFoudScoreboard; }

	public int ID { get => id; }

	private Interaction secretInteraction;
	private BoxCollider2D boxCollider2D;
	private SpriteRenderer spriteRenderer;

	private void OnDisable()
	{
		secretInteraction.OnInteraction -= CollectSecret;
	}

	private void Start()
	{
		secretInteraction = GetComponent<Interaction>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		secretInteraction.OnInteraction += CollectSecret;

		LevelEvent.RegisterSecret(this);
	}

	public void ResetSecret()
	{
		boxCollider2D.enabled = true;
		spriteRenderer.enabled = true;
	}

	private void CollectSecret()
	{
		LevelEvent.SecretFound(id);
		boxCollider2D.enabled = false;
		spriteRenderer.enabled = false;
	}
}
