using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
	[SerializeField]
	private int maxHealth;

	public int currentHealth;

	public UnityEvent OnHitWithReference, OnDeathWithReference;

	private bool isDead = false;

	public void InitializeHealth(int healthValue)
	{
		currentHealth = healthValue;
		maxHealth = healthValue;
		isDead = false;
	}

	public void GetHit(int amount)
	{
		if (isDead)
			return;

		currentHealth -= amount;

		if (currentHealth > 0)
		{
			OnHitWithReference?.Invoke();
			
		}
		else
		{
			OnDeathWithReference?.Invoke();
			isDead = true;
		}
	}    
}
