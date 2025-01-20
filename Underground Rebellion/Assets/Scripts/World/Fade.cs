using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fade : MonoBehaviour
{
    public float fadeDuration = 0.2f; // Duration of the fade effect
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
            StartCoroutine(FadeOut());
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

        // Disable the TilemapRenderer after fading
        tilemapRenderer.enabled = false;
    }
}
