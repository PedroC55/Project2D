using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private Stalactite stalactite; // Reference to the Stalactite script
	private void Awake()
	{
		stalactite = GetComponentInParent<Stalactite>();
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the detected collider belongs to the ground layer
        if (((1 << other.gameObject.layer) & stalactite.groundLayer) != 0)
        {
            stalactite.StopStalactite(); // Stop the stalactite from falling
        }
    }
}
