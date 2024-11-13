using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionAnimatorEvent : MonoBehaviour
{
    public abstract void OnActionStart();
	public abstract void OnActionEnd();

}
