using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
	public delegate void InteractionHandler();
	public event InteractionHandler OnInteraction;

    public GameObject buttonIcon;

    private bool playerInRange = false;

	[SerializeField]
	private InputActionReference interaction;

	private void OnEnable()
	{
		interaction.action.performed += PlayerInteracted;
	}

    public bool PlayerInRange() 
    { 
        return playerInRange; 
    }

    private void PlayerInteracted(InputAction.CallbackContext context)
    {
        if (playerInRange)
        {
            OnInteraction?.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player is within range
        if (other.CompareTag("Player") && !other.isTrigger)
        {
			// Show the icon and the E key prompt
			buttonIcon.SetActive(true);
			playerInRange = true;
		}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Hide the icon and E key prompt when the player leaves
        if (other.CompareTag("Player") && !other.isTrigger)
        {
			buttonIcon.SetActive(false);
			playerInRange = false;
		}
    }
}