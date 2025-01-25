using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Yarn.Unity;

public class TerrainScroller : MonoBehaviour
{
	private bool startDestroing = false;
	private bool stopScrolling = false;

	void OnEnable()
	{
		CutsceneEvent.OnStartFall += PlayerFalling;
		CutsceneEvent.OnStopScroller += StopScroller;
	}

	void OnDisable()
	{
		CutsceneEvent.OnStartFall -= PlayerFalling;
		CutsceneEvent.OnStopScroller -= StopScroller;
	}

	// Update is called once per frame
	void Update()
    {
		if (stopScrolling)
			return;

		transform.Translate(Vector3.up * Time.deltaTime * 15f, Space.World);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Finish"))
		{
			if (startDestroing)
			{
				CutsceneEvent.DestroyScroller();
				Destroy(gameObject);
				return;
			}

			var tempPosition = transform.position;
			tempPosition.y -= 96f;
			transform.position = tempPosition;
		}
	}

	private void PlayerFalling()
	{
		startDestroing = true;
	}

	private void StopScroller()
	{
		stopScrolling = true;
	}
}
