using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{

    public GameObject pauseMenu;
    public static bool isPaused;


    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        foreach (Transform child in pauseMenu.transform)
        {
            if (child.gameObject.name == "Menu")
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                foreach (Transform littlechild in child.transform)
                {
                    if (littlechild.gameObject.name == "System" || littlechild.gameObject.name == "Error Prompt")
                    {
                        littlechild.gameObject.SetActive(false);
                    }
                    else
                    {
                        littlechild.gameObject.SetActive(true);
                    }
                }
                child.gameObject.SetActive(false);
            }
            
        }
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
		isPaused = false;
		SceneManager.LoadScene("Main Menu");
    }
}
