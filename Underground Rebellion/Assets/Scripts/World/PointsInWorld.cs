using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsInWorld : MonoBehaviour
{
	[SerializeField]
	private TMP_Text pointsText;

	[SerializeField]
	private Color negativeScore;
	[SerializeField]
	private Color positiveScore;

	private Color currentColor;
	private float timeToDisapear = 2f;
	private float timeElapsed = 0f;
	void Update()
	{
		if (timeElapsed < timeToDisapear)
		{
			float lerpvalue = timeElapsed / timeToDisapear;
			float currentAlpha = Mathf.Lerp(1, 0, lerpvalue);
			timeElapsed += Time.deltaTime;

			currentColor.a = currentAlpha;
			pointsText.color = currentColor;

			gameObject.transform.Translate(Vector3.up * Time.deltaTime);
		}
	}

	public void InitScore(int score, PointsTypes pointType)
	{
		bool isPositive = score > 0;
		string sign = isPositive ? "+" : "-";
		string text = "";
		switch (pointType)
		{
			case PointsTypes.Defeat:
				text = "Enemy Defeated";
				break;
			case PointsTypes.Undetected:
				text = "Undetected";
				break;
			case PointsTypes.Aggro:
				text = "Enemy Aggroed";
				break;
			case PointsTypes.Stun:
				text = "Enemy Stunned";
				break;
			case PointsTypes.Secret:
				text = "Secret Found!";
				break;
		}

		pointsText.text = $"{sign}{score}\n{text}";

		currentColor = isPositive ? positiveScore : negativeScore;

		Destroy(gameObject, timeToDisapear);
	}
}
