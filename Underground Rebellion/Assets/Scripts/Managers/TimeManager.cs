using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public static TimeManager Instance { get; private set; }

	private void Awake()
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

	public void StopTime(float slowDownLength)
	{
		Time.timeScale = 0f;
		StartCoroutine(ResetTime(slowDownLength));
	}

	public void DoSlowMotion(float slowDownFactor, float slowDownLength)
	{
		Time.timeScale = slowDownFactor;
		Time.fixedDeltaTime = Time.timeScale * 0.2f;
		StartCoroutine(ResetTime(slowDownLength));
	}

	private IEnumerator ResetTime(float slowDownLength)
	{
		if (slowDownLength < 0)
			slowDownLength = 0;

		yield return new WaitForSecondsRealtime(slowDownLength);
		Time.timeScale = 1;
	}
}
