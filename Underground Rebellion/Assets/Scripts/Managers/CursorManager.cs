using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CursorManager : MonoBehaviour
{
	void Update()
	{
		Scene currentScene = SceneManager.GetActiveScene();
		UpdateCursorState(currentScene);
	}

	// Atualiza o estado do cursor com base no estado do jogo
	private void UpdateCursorState(Scene currentScene)
	{
		if (PauseMenuManager.isPaused || currentScene.name == "Main Menu"
			|| currentScene.name == "Scoreboard" || DialogueManager.Instance.IsDialogueRunning())
		{
			Cursor.visible = true; // Mostra o cursor
			Cursor.lockState = CursorLockMode.None; // Permite o cursor se mover livremente

		}
		else
		{
			Cursor.visible = false; // Esconde o cursor
			Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da tela
		}
	}
}