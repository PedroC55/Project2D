using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryEvent : MonoBehaviour
{
    public delegate void ParryHandler(int amount, GameObject receiver);
	public static event ParryHandler OnParry;

	public static void Parry(int amount, GameObject receiver)
	{
		OnParry?.Invoke(amount, receiver);
	}
}
