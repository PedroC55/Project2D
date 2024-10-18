using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{
	private LayerMask detectionMask;
	private EnemyAI enemyAI;
	private void Awake()
	{
		detectionMask = LayerMask.GetMask("Ground", "Player");
		enemyAI = GetComponentInParent<EnemyAI>();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			RaycastHit2D detectedObject = Physics2D.Raycast(transform.position, collision.transform.position - transform.position, Mathf.Infinity, detectionMask);
			
			if(detectedObject.collider != null && detectedObject.collider.gameObject.CompareTag("Player"))
			{
				enemyAI.Aggroed(collision.gameObject.transform);
				GetComponent<Collider2D>().enabled = false;
			}
		}
	}

	public void ActiveLineOfSight()
	{
		GetComponent<Collider2D>().enabled = true;
	}
	
}
