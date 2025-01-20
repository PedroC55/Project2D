using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
	[SerializeField]
	private int damage;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy") | collision.CompareTag("Door"))
		{
			HitEvent.GetHit(damage, transform.parent.gameObject, collision.gameObject);
        }
	}
}
