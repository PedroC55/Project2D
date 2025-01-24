using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private int roomId;

    private List<EnemyAI> enemyAIList = new();
    public GameObject virtualCam;

    // Reference to the background GameObject
    [SerializeField]
    private Transform background;

    // Intensidade do movimento do background (quanto menor, mais lento ele se move)
    [SerializeField]
    private float parallaxEffectMultiplier = 0.2f;

    // Referência ao jogador
    [SerializeField]
    private Transform player;

    // Posição inicial do background
    private Vector3 initialBackgroundPosition;

    // Posição inicial do jogador ao entrar na sala
    private Vector3 initialPlayerPosition;

    // Flag para verificar se o jogador está na sala
    private bool playerInRoom = false;

    private void Start()
    {
        // Salva a posição inicial do background
        if (background != null)
        {
            initialBackgroundPosition = background.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(true);

            CanvasEvent.UpdateMap(roomId);

            // Reposiciona o background para a posição desta sala
            if (background != null)
            {
                background.position = this.gameObject.transform.position;
                initialBackgroundPosition = background.position; // Atualiza a posição inicial do background
                Debug.Log("Tempo dentro do trigger:" + Time.fixedTime);
            }
            // Salva a posição inicial do jogador ao entrar na sala
            if (player != null)
            {
                initialPlayerPosition = player.position;
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

    private void LateUpdate()
    {
        // Aplica o efeito de parallax somente se o jogador e o background estiverem configurados
        if (player != null && background != null)
        {
            // Calcula a diferença de posição do jogador em relação ao ponto inicial
            Vector3 playerOffset = player.position - initialPlayerPosition;
            Debug.Log("Tempo dentro do Update:" + Time.fixedTime);
            // Move o background em função do deslocamento do jogador (efeito de parallax)
            Vector3 parallaxPosition = initialBackgroundPosition + playerOffset * parallaxEffectMultiplier;

            // Atualiza a posição do background
            background.position = new Vector3(parallaxPosition.x, parallaxPosition.y, initialBackgroundPosition.z);
        }
    }
}