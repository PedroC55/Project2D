using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AgentAnimations : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void WalkingAnimation(Vector2 movementInput)
	{
		LookDirection(movementInput);
		animator.SetInteger("state", 1);
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

	//Caso queira colocar uma animação de morte que sabe da direção que vem a morte, para adicionar particulas ou knockback, é bom ter o sender no paramentro
	public void DeathAnimation(GameObject sender)
	{
		animator.SetTrigger("death");
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}