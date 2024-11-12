using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorInteraction : MonoBehaviour
{
    private Animator animator;
    private bool playerNearby = false;  // To track if the player is in range

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for input if player is nearby
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Disappear();
        }
    }

    private void Disappear()
    {
        // Play the jump animation
        animator.SetTrigger("Press");

        // Destroy the game object after the animation
        
        Destroy(gameObject, 1f); // Adjust delay as per jump animation length
    }

    // Detect when player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Check if it's the player
        {
            playerNearby = true;
        }
    }

    // Detect when player leaves the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}