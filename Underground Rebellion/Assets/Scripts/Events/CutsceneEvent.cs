using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvent : MonoBehaviour
{
	public delegate void StartFallHandler();
	public static event StartFallHandler OnStartFall;
	
	public delegate void DestroyScrollerHandler();
	public static event DestroyScrollerHandler OnDestroyScroller;

	public delegate void StopScrollerHandler();
	public static event StopScrollerHandler OnStopScroller;

	public static void StartFall()
	{
		OnStartFall?.Invoke();
	}

	public static void DestroyScroller()
	{
		OnDestroyScroller?.Invoke();
	}

	public static void StopScroller()
	{
		OnStopScroller?.Invoke();
	}
}
