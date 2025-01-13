using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject mapUI;
    public GameObject mapOverlay;

    private bool isMapActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!PauseMenuManager.isPaused)
            {
                ToggleMap();
            }
        }
    }

    public void ToggleMap()
    {
        isMapActive = !isMapActive;
        mapUI.SetActive(isMapActive);
        mapOverlay.SetActive(isMapActive);

        // Optionally pause the game
        Time.timeScale = isMapActive ? 0 : 1;
    }
}