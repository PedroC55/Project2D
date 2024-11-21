using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimatiorEvents : MonoBehaviour
{

	[Serializable]
	public class ActionAnimator
	{
		/// <summary>
		/// O nome da a��o chamada no animador
		/// </summary>
		public string Name;

		/// <summary>
		/// Objecto da a��o chamada no animador
		/// </summary>
		public ActionAnimatorEvent ActionEvent;
	}

	/// <summary>
	/// Lista de todas a��es utilizadas nos eventos do animador deste personagem
	/// </summary>
	public ActionAnimator[] ActionAnimatorArray;

	/// Onde realmente mantem os toggles
	private readonly Dictionary<string, ActionAnimatorEvent> actionsAnimatorDict = new();

	private void Awake()
	{
		foreach (ActionAnimator actionAnim in ActionAnimatorArray)
		{
			actionsAnimatorDict.Add(actionAnim.Name, actionAnim.ActionEvent);
		}

		ActionAnimatorArray = null;
	}

	public void OnActionStart(string eventName)
    {
		actionsAnimatorDict[eventName].OnActionStart();
	}

	public void OnActionEnd(string eventName)
	{
		actionsAnimatorDict[eventName].OnActionEnd();
	}
}
