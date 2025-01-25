using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private int roomId;

    private List<EnemyAI> enemyAIList = new();
    public GameObject virtualCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(true);

            CanvasEvent.UpdateMap(roomId);
            // LevelEvent.PlayerEnterRoom(collision.gameObject);

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

}