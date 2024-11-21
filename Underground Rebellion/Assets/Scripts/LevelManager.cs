using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	private int croissants = 0;

	private Transform lastSavePosition;

	
	/* inimios = {id: {isdead}}
	 * objetos = {id:}
	 */

	private void OnEnable()
	{
		LevelEvent.OnWinCroissant += WinCroissant;
		LevelEvent.OnPlayerSave += PlayerSave;
		LevelEvent.OnPlayerDied += PlayerDied;
	}

	private void OnDisable()
	{
		LevelEvent.OnWinCroissant -= WinCroissant;
		LevelEvent.OnPlayerSave -= PlayerSave;
		LevelEvent.OnPlayerDied -= PlayerDied;
	}

	public void WinCroissant()
	{
		croissants++;
		CanvasEvent.WinCroissant(croissants);
	}

	public void PlayerSave(Transform player)
	{
		lastSavePosition = player;

		Debug.Log("Salvou");

		//Salvar o transform que  player se encontra do save

		//No EnemiesManager salvar quais deles estão vivos e mortos num dicionario

		//No ObjectsManager salvar o estado dos objetos e a posição dos objetos que se mexem
	}

	public void PlayerDied()
	{
		LevelEvent.ResetPlayer(lastSavePosition);

		//Resta os inimigos
		
		//Reseta os objetos
	}
}
