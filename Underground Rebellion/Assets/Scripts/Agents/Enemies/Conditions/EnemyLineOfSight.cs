using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{
	private LayerMask detectionMask;
	private EnemyAI enemyAI;
	public Animator aggroMarkAnimator;

	private float loseAggroTime = 2f;
	private float currentTime = 0f;
	private Transform player;

	private void Awake()
	{
		detectionMask = LayerMask.GetMask("Ground", "Player");
		enemyAI = GetComponentInParent<EnemyAI>();
	}

	private void FixedUpdate()
	{
		if (player)
		{
			RaycastHit2D detectedObject = Physics2D.Raycast(transform.position, player.position - transform.position, Mathf.Infinity, detectionMask);
			if (detectedObject.collider.CompareTag("Player"))
			{
				currentTime = 0;
				aggroMarkAnimator.SetBool("LossingAggro", false);
			}
			else
			{
				//Iniciar animação
				currentTime += Time.deltaTime;
				aggroMarkAnimator.SetBool("LossingAggro", true);
				if (currentTime > loseAggroTime)
				{
					player = null;
					aggroMarkAnimator.SetBool("LossingAggro", false);
					enemyAI.ResetEnemy();
				}
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			RaycastHit2D detectedObject = Physics2D.Raycast(transform.position, collision.transform.position - transform.position, Mathf.Infinity, detectionMask);
			
			if(detectedObject.collider != null && detectedObject.collider.gameObject.CompareTag("Player"))
			{
				if (enemyAI.CanAggro())
				{
					player = collision.gameObject.transform;
					aggroMarkAnimator.SetTrigger("Aggro");
					enemyAI.Aggroed(player);
					GetComponent<Collider2D>().enabled = false;
				}
			}
		}
	}

	public void ActiveLineOfSight()
	{
		GetComponent<Collider2D>().enabled = true;
	}
	
}
