using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsEvent : MonoBehaviour
{
	public delegate void KeyRebindHandler(InputAction action);
	public static event KeyRebindHandler OnRebind;

	public static void Rebind(InputAction action)
	{
		OnRebind?.Invoke(action);
	}
}
