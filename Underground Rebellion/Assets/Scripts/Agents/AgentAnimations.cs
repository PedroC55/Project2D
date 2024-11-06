using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class AgentAnimations : MonoBehaviour
{
	private Animator animator;
	public Transform wallCheck;
	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void WalkingAnimation(Vector2 movementInput)
	{
		LookDirection(movementInput);
		animator.SetInteger("state", 1);
	}

	public void IdleAnimation()
	{
		animator.SetInteger("state", 0);
	}

	public void JumpingAnimation()
	{
		animator.SetInteger("state", 2);
	}

	public void FallingAnimation()
	{
		animator.SetInteger("state", 3);
	}


	public void LookDirection(Vector2 direction)
	{
		var scale = transform.parent.localScale;
		if (direction.x > 0)
		{
			scale.x = 1;
		}
		else if (direction.x < 0)
		{
			scale.x = -1;
		}

		transform.parent.localScale = scale;
		
	}

	public void ChangeLookDirection(Vector2 wallJumpVector)
    {
		var scale = transform.parent.localScale;
		
		if (wallJumpVector.x > 0)
        {
			scale.x = 1;
		}
		else if (wallJumpVector.x < 0)
		{
			scale.x = -1;
		}
		transform.parent.localScale = scale;
		Debug.Log(transform.parent.localScale);

	}

	public float getDirection()
	{
		return transform.parent.localScale.x;
	}


	public void AttackAnimation(string triggerName)
	{
		animator.SetTrigger(triggerName);
	}

	//Caso queira colocar uma animação de morte que sabe da direção que vem a morte, para adicionar particulas ou knockback, é bom ter o sender no paramentro
	public void DeathAnimation()
	{
		animator.SetTrigger("death");
	}

	public void StunAnimation()
	{
		animator.SetTrigger("stun");
	}

	public void RestartLevel()
	{
		StartCoroutine(RestartLevelWithDelay(5f));
	}

	private IEnumerator RestartLevelWithDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}