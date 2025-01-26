using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private Rigidbody2D rb2d;
	private EnemyAI shooter;

	[SerializeField]
	private int damage;
	private Vector2 forceDirection = Vector2.zero;
	private float projectileForce;
	private bool repelling = false;

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

	public void SetForce(float force, Transform target)
	{
		projectileForce = force;
		Vector2 direction = (target.position - transform.position).normalized;

		forceDirection = direction * force;
	}

	public void RepelProjectile()
	{
		SetForce(projectileForce * 1.5f, shooter.transform);
		repelling = true;
	}

	public void DestroyOnHit()
	{
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!repelling && collision.gameObject.GetInstanceID() == shooter.gameObject.GetInstanceID())
		{
			return;
		}

		if (collision.gameObject.CompareTag("Player"))
		{
			HitEvent.GetHit(damage, gameObject, collision.gameObject);
			return;
		}
		else if(collision.gameObject.CompareTag("Enemy"))
		{
			EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
			if (enemy.HasEnergy())
				enemy.DecreaseEnergy(damage, enemy.gameObject);
			else
				HitEvent.GetHit(damage, gameObject, collision.gameObject);
		}

		Destroy(gameObject);
	}
}
