using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomFade : MonoBehaviour
{
    public float fadeDuration = 2f; // Duration of the fade effect
    private TilemapRenderer tilemapRenderer;
    private Material tilemapMaterial;
    private Color initialColor;

    private void Start()
    {
        // Get the TilemapRenderer and its material
        tilemapRenderer = GetComponent<TilemapRenderer>();
        if (tilemapRenderer != null)
        {
            tilemapMaterial = tilemapRenderer.material;
            initialColor = tilemapMaterial.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player touches the tilemap
        if (other.CompareTag("Player") && tilemapMaterial != null)
        {
            StopAllCoroutines(); // Stop any running coroutines to prevent conflicts
            StartCoroutine(FadeOut());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player leaves the tilemap
        if (other.CompareTag("Player") && tilemapMaterial != null)
        {
            StopAllCoroutines(); // Stop any running coroutines to prevent conflicts
            StartCoroutine(FadeIn());
        }
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / fadeDuration);
            tilemapMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Disable the TilemapRenderer after fading out
        tilemapRenderer.enabled = false;
    }

    private System.Collections.IEnumerator FadeIn()
    {
        tilemapRenderer.enabled = true; // Ensure the renderer is enabled before fading in
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, initialColor.a, elapsedTime / fadeDuration);
            tilemapMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }
    }
}