using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stalactite : MonoBehaviour
{
    public float fallSpeed = 5f; // Speed at which the stalactite falls
    public LayerMask playerLayer; // Layer for the player
    public LayerMask groundLayer; // Layer for the ground
    public float shakeDuration = 3f; // Duration of the shaking in seconds
    public float shakeMagnitude = 0.1f; // Magnitude of the shake effect

    public BoxCollider2D damageBoxCollider;

    private Rigidbody2D rb;
    private bool isFalling = false;
    private bool detected = false;
    private bool onGround = false;
    private Vector3 originalPosition;

    public bool fallOnlyOnEnemyDead = false;
    public GameObject enemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.localPosition;
        Physics2D.IgnoreLayerCollision(7, 9, false);
    }

    void Update()
    {

        if (enemy != null && !enemy.activeSelf && !isFalling && !onGround && fallOnlyOnEnemyDead) // If enemy is destroyed or disabled
        {
            StartCoroutine(ShakeAndFall());
        }


        if (isFalling)
        {
            rb.velocity = new Vector2(0, -fallSpeed); // Make the stalactite fall
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger zone using layers
        if(((1 << other.gameObject.layer) & playerLayer) != 0 && !detected && !fallOnlyOnEnemyDead)
        {
            detected = true;
            StartCoroutine(ShakeAndFall()); // Start the shaking effect
        }
    }

    IEnumerator ShakeAndFall()
    {
        float elapsed = 0f;

        // Shake the stalactite for the specified duration
        while (elapsed < shakeDuration)
        {
            float x = originalPosition.x;
            float y = originalPosition.y + Random.Range(-shakeMagnitude, shakeMagnitude);
            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Reset position and start falling
        transform.localPosition = originalPosition;
        isFalling = true;
        rb.bodyType = RigidbodyType2D.Dynamic; // Set to Dynamic to enable falling
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((((1 << collision.gameObject.layer) & playerLayer) != 0) & !onGround)
        {
            Physics2D.IgnoreLayerCollision(7, 9);
            isFalling = true;
        }
    }

    public void StopStalactite()
    {
        isFalling = false;
        Physics2D.IgnoreLayerCollision(7, 9, false);
        rb.bodyType = RigidbodyType2D.Static; // Change to Static
        gameObject.layer = LayerMask.NameToLayer("Ground"); // Change layer to Ground
        onGround = true;
        damageBoxCollider.enabled = false;
    }
}
