using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private Rigidbody2D rb2d;
	private EnemyAI shooter;

	[SerializeField]
	private int damage;
	private Vector2 forceDirection = Vector2.zero;

	private void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}
	private void FixedUpdate()
	{
		rb2d.velocity = forceDirection;
	}

	public void SetShotter(EnemyAI enemy)
	{
		shooter = enemy;
	}

	public void SetForce(Vector2 force)
	{
		forceDirection = force;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.GetInstanceID() == shooter.gameObject.GetInstanceID())
		{
			return;
		}

		if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
		{
			Debug.Log("Projetil deu dano");
			//collision.gameObject.GetComponent<Health>().GetHit(damage, gameObject);
			HitEvent.GetHit(damage, shooter.gameObject, collision.gameObject);
		}

		Destroy(gameObject);
	}
}
