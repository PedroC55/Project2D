using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
	[SerializeField]
	private GameObject canvas;
	[SerializeField]
	private Sprite fixedElevatorSprite;

	private Interaction elevatorInteraction;
	
	private void OnEnable()
	{
		LevelEvent.OnFixElevator += FixElevator;
	}

	private void OnDisable()
	{
		LevelEvent.OnFixElevator -= FixElevator;
		elevatorInteraction.OnInteraction -= UseElevator;
	}

	private void Start()
	{
		elevatorInteraction = GetComponent<Interaction>();
		elevatorInteraction.OnInteraction += UseElevator;
	}

	void Update()
	{
		// Check if the player is in range and presses the "E" key
		if (elevatorInteraction.PlayerInRange() && Input.GetKeyDown(KeyCode.E))
		{
			// Make the box disappear by disabling the GameObject
			gameObject.SetActive(false);
		}
	}

	public void FixElevator()
	{
		GetComponent<SpriteRenderer>().sprite = fixedElevatorSprite;
		GetComponent<Interaction>().enabled = true;
		GetComponent<BoxCollider2D>().enabled = true;
		canvas.SetActive(false);
	}

	public void UseElevator()
	{
		CanvasEvent.FinishLevel();
	}
}
