using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        if (SettingsManager.Instance.SkipDialogues)
        {
			SceneManager.LoadScene("Level 1");
        }
        else
        {
			SceneManager.LoadScene("Initial Cutscene");
		}
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
