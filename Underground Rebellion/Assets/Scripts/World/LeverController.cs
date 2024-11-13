using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    public Transform platform;       // The platform that will move
    public Transform positionObject1; // First position object
    public Transform positionObject2; // Second position object
    public float moveSpeed = 2f;     // Speed at which the platform moves

    private bool isPlayerNearby = false; // Checks if the player is in range
    private bool moveToPosition1 = true; // Direction of platform movement

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Toggle the movement direction
            moveToPosition1 = !moveToPosition1;
        }

        // Move the platform to the selected position
        if (moveToPosition1)
        {
            platform.position = Vector2.MoveTowards(platform.position, positionObject1.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            platform.position = Vector2.MoveTowards(platform.position, positionObject2.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}