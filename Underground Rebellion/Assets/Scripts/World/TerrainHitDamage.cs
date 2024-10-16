using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHitDamage : MonoBehaviour
{
	[SerializeField]
	private int damage;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<Health>().GetHit(damage, gameObject);
		}
	}
}
