using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int croissants = 0;

	private void OnEnable()
	{
		LevelEvent.OnWinCroissant += WinCroissant;
	}

	private void OnDisable()
	{
		LevelEvent.OnWinCroissant -= WinCroissant;
	}

	public void WinCroissant()
    {
        croissants++;
        CanvasEvent.WinCroissant(croissants);
    }
}
