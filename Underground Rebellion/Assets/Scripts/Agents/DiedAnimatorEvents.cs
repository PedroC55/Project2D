using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DiedAnimatorEvents : ActionAnimatorEvent
{
	public override void OnActionStart()
	{
		LevelEvent.PlayerDied();
	}

	public override void OnActionEnd() { }
}
