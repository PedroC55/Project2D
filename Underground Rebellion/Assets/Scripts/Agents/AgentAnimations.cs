using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class AgentAnimations : MonoBehaviour
{
	private Animator animator;

	private enum MovementState { idle, running, jumping, falling }

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void WalkingAnimation(Vector2 movementInput)
	{
		LookDirection(movementInput);
		animator.SetInteger("state", (int)MovementState.running);
	}

	public void IdleAnimation()
	{
		animator.SetInteger("state", (int) MovementState.idle);
	}

	public void JumpingAnimation()
	{
		animator.SetInteger("state", (int)MovementState.jumping);
	}

	public void FallingAnimation()
	{
		animator.SetInteger("state", (int)MovementState.falling);
	}

	public void LookDirection(Vector2 direction)
	{
		var scale = transform.parent.localScale;

		Quaternion agentRotation = transform.rotation;
		Vector2 correctFaceDirection = Quaternion.Inverse(agentRotation) * direction;

		if (correctFaceDirection.x > 0)
		{
			scale.x = 1;
		}
		else if (correctFaceDirection.x < 0)
		{
			scale.x = -1;
		}

		transform.parent.localScale = scale;
	}

	public float GetDirection()
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

	public void ParryAnimation()
	{
		animator.SetTrigger("parry");
	}

	public void AttackAnimation()
	{
		animator.SetTrigger("attack");
	}

	//public void RestartLevel()
	//{
	//	StartCoroutine(RestartLevelWithDelay(2f));
	//}

	//private IEnumerator RestartLevelWithDelay(float delay)
	//{
	//	yield return new WaitForSeconds(delay);
	//	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	//}
}