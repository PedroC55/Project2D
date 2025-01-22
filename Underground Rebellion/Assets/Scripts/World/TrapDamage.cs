using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField]
    private int damage; // Amount of damage dealt to the player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is the player
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
			HitEvent.GetHit(damage, transform.parent.gameObject, collision.gameObject);
        }
    }
}
