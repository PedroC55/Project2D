using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretsScoreboard : MonoBehaviour
{
    [SerializeField] private Image firstSecret;
	[SerializeField] private Image secondSecret;
	[SerializeField] private Image thirdSecret;
	[SerializeField] private Image forthSecret;

	private void Start()
	{
		foreach(KeyValuePair<int, SecretObject> secret in ScoreManager.Instance.Secrets)
		{
			switch (secret.Key)
			{
				case 1:
					firstSecret.sprite = secret.Value.Found ? secret.Value.SecretFoundSprite : secret.Value.SecretNotFoundSprite;
					break;
				case 2:
					secondSecret.sprite = secret.Value.Found ? secret.Value.SecretFoundSprite : secret.Value.SecretNotFoundSprite;
					break;
				case 3:
					thirdSecret.sprite = secret.Value.Found ? secret.Value.SecretFoundSprite : secret.Value.SecretNotFoundSprite;
					break;
				case 4:
					forthSecret.sprite = secret.Value.Found ? secret.Value.SecretFoundSprite : secret.Value.SecretNotFoundSprite;
					break;
			}
		}
	}

}
