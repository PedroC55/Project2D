using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumplableRoom : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !collision.isTrigger)
		{
			collision.GetComponent<PlayerInput>().AddWallJump();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !collision.isTrigger)
		{
			collision.GetComponent<PlayerInput>().RemoveWallJump();
		}
	}
}
