using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
	[SerializeField]
	private int damage;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Health>() != null)
		{
			//Debug.Log("Acertou o ataque");
			//collision.gameObject.GetComponent<Health>().GetHit(damage, gameObject);
			HitEvent.GetHit(damage, gameObject, collision.gameObject);
        }
	}
}
