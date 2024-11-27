using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteTipDamage : MonoBehaviour
{
    [SerializeField]
    private int damage; // Amount of damage dealt to the player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Try to get the Health component from the player and apply damage
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.GetHit(damage);
            }
        }
    }
}
