using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private int roomId;

    private List<EnemyAI> enemyAIList = new();
    public GameObject virtualCam;
    public Transform playerIcon; // UI Image representing the player

    // Reference to the Map UI Manager or similar component
    public GameObject mapUI;
    private bool isVisited = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(true);

            // Mark the room as visited and update the map
            if (!isVisited)
            {
                isVisited = true;
                UpdateMapVisibility();
                UpdatePlayerIcon();
            }
            else
            {
                UpdatePlayerIcon();
            }
        }
        else if (collision.CompareTag("Enemy") && !collision.isTrigger)
        {
            enemyAIList.Add(collision.GetComponent<EnemyAI>());
            collision.GetComponent<EnemyAI>().SetRoomID(roomId);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(false);
            LevelEvent.ResetRoomEnemies(roomId);
        }
    }

    private void UpdateMapVisibility()
    {
        Transform roomIcon = mapUI.transform.Find("Room" + roomId);
        if (roomIcon != null)
        {
            roomIcon.gameObject.SetActive(true); // Show the room icon on the map
        }
    }

    private void UpdatePlayerIcon()
    {
        playerIcon.position = mapUI.transform.Find("Room" + roomId).position;
        Debug.Log("Icon:" + playerIcon.position + ", Room " + roomId + ":" + mapUI.transform.Find("Room" + roomId).position);
    }
}