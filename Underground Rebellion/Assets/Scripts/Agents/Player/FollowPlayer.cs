using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Assign the player transform in the Inspector

    void Update()
    {
        // Set the position of buttonE_O to match the player's position
        transform.position = new Vector3(player.position.x, player.position.y + 1.75f, player.position.z);
    }
}
