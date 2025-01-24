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

    // Refer�ncia ao jogador
    [SerializeField]
    private Transform player;

    // Posi��o inicial do background
    private Vector3 initialBackgroundPosition;

    // Posi��o inicial do jogador ao entrar na sala
    private Vector3 initialPlayerPosition;

    // Flag para verificar se o jogador est� na sala
    private bool playerInRoom = false;

    private void Start()
    {
        // Salva a posi��o inicial do background
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

            // Reposiciona o background para a posi��o desta sala
            if (background != null)
            {
                background.position = this.gameObject.transform.position;
                initialBackgroundPosition = background.position; // Atualiza a posi��o inicial do background
                Debug.Log("Tempo dentro do trigger:" + Time.fixedTime);
            }
            // Salva a posi��o inicial do jogador ao entrar na sala
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
            // Calcula a diferen�a de posi��o do jogador em rela��o ao ponto inicial
            Vector3 playerOffset = player.position - initialPlayerPosition;
            Debug.Log("Tempo dentro do Update:" + Time.fixedTime);
            // Move o background em fun��o do deslocamento do jogador (efeito de parallax)
            Vector3 parallaxPosition = initialBackgroundPosition + playerOffset * parallaxEffectMultiplier;

            // Atualiza a posi��o do background
            background.position = new Vector3(parallaxPosition.x, parallaxPosition.y, initialBackgroundPosition.z);
        }
    }
}