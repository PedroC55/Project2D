using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    public GameObject breakableIcon; // Assign the BreakableIcon here
    

    void Start()
    {
        // Hide the icon initially
        breakableIcon.SetActive(false);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player is within range
        if (other.CompareTag("Player"))
        {
            // Show the icon and the E key prompt
            breakableIcon.SetActive(true);
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Hide the icon and E key prompt when the player leaves
        if (other.CompareTag("Player"))
        {
            breakableIcon.SetActive(false);
           
        }
    }
}