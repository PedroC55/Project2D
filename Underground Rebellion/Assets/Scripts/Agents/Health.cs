using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
	public UnityEvent OnHitWithReference, OnDeathWithReference;
	
	[SerializeField]
	private int maxHealth;
	private int currentHealth;

	private bool isDead = false;

	private void Start()
	{
		currentHealth = maxHealth;
	}

	public void ResetHealth()
	{
		currentHealth = maxHealth;
		isDead = false;
	}

	public int GetHit(int amount)
	{
		if (isDead)
			return currentHealth;

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

		SoundManager.PlaySound(SoundType.DAMAGE);
		Debug.Log("DAMAGE");

		return currentHealth;
	}   
	
	public bool IsDead()
	{
		return isDead;
	}

	public int GetMaxHealth()
	{
		return maxHealth;
	}
}
