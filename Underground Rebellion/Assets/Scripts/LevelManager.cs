using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	private int croissants = 0;

	[SerializeField]
	private Transform lastSavePosition;

	private Dictionary<int, string> objectsState = new();
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

	private void InitializeObject(GameObject worldObject, string state)
	{
		objectsState.Add(worldObject.GetInstanceID(), state);
	}

	private void WinCroissant()
	{
		croissants++;
		CanvasEvent.WinCroissant(croissants);
	}

	private void PlayerSave(Transform player)
	{
		lastSavePosition = player;

		Debug.Log("Salvou");

		//Salvar o transform que  player se encontra do save

		//No EnemiesManager salvar quais deles estão vivos e mortos num dicionario

		//No ObjectsManager salvar o estado dos objetos e a posição dos objetos que se mexem
	}

	private void PlayerDied()
	{
		LevelEvent.ResetPlayer(lastSavePosition);

		//Resta os inimigos
		
		//Reseta os objetos
	}
}
