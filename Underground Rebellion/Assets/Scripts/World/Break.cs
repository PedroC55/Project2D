using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        // Check if the player is in range and presses the "E" key
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Make the box disappear by disabling the GameObject
            gameObject.SetActive(false);
            SoundManager.PlaySound(SoundType.BREAK);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the trigger
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
