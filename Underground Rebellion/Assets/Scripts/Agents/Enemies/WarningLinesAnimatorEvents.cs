using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLinesAnimatorEvents : ActionAnimatorEvent
{
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public override void OnActionStart()
	{
		animator.SetTrigger("Start");
	}

	public override void OnActionEnd() { }
}
