using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackDamage : MonoBehaviour
{
	[SerializeField]
	private int damage;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
		{
			HitEvent.GetHit(damage, transform.parent.gameObject, collision.gameObject);
		}
	}
}
