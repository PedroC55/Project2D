using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorInteraction : MonoBehaviour
{
    private Animator animator;

	private Interaction survivorInteraction;

	private void OnDisable()
	{
		survivorInteraction.OnInteraction -= FixElevator;
	}

	private void Start()
	{
        animator = GetComponent<Animator>();
		survivorInteraction = GetComponent<Interaction>();
		survivorInteraction.OnInteraction += FixElevator;
	}

    private void FixElevator()
    {
		LevelEvent.FixElevator();
        animator.SetTrigger("Press");

        Destroy(gameObject, 1f);
    }
}