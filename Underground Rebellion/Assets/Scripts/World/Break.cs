using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{

    public GameObject[] woodenPiecePrefabs; // Assign your wooden piece prefabs in the inspector
    public int numberOfPieces = 5; // Number of pieces to spawn
    public Vector2 spawnAreaSize = new Vector2(2f, 2f); // Area where the pieces will spawn
    public float explosionForce = 5f; // Force applied to scatter pieces
    public float pieceLifetime = 2f;
    private void OnEnable()
    {
        HitEvent.OnHit += HandleHit;
    }

    private void OnDisable()
    {
        HitEvent.OnHit -= HandleHit;
    }

    private void HandleHit(int damage, GameObject sender, GameObject receiver)
    {
        // Check if the receiver of the hit is this object
        if (sender.CompareTag("Player") && receiver.GetInstanceID() == gameObject.GetInstanceID())
        {
            // Make the object disappear
            gameObject.SetActive(false);
            // Play sound effect (if applicable)
            SoundManager.Instance.PlaySound(SoundType.BREAK);
            for (int i = 0; i < numberOfPieces; i++)
            {
                // Choose a random piece prefab
                GameObject piece = woodenPiecePrefabs[Random.Range(0, woodenPiecePrefabs.Length)];

                // Spawn the piece at a random position near the door
                Vector2 randomPosition = (Vector2)transform.position + new Vector2(
                    Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                    Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
                );

                GameObject spawnedPiece = Instantiate(piece, randomPosition, Quaternion.identity);

                // Add a random force to simulate the explosion effect
                Rigidbody2D rb = spawnedPiece.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 randomForce = new Vector2(
                        Random.Range(-explosionForce, explosionForce),
                        Random.Range(-explosionForce, explosionForce)
                    );
                    rb.AddForce(randomForce, ForceMode2D.Impulse);
                }

                Destroy(spawnedPiece, pieceLifetime);
            }
            
        }
    }
}
