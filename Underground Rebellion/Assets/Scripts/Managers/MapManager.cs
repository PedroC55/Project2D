using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject mapUI;
    public GameObject mapOverlay;
    public GameObject playerIcon;

    public static bool isMapActive = false;

    private void Start()
    {
        mapUI.SetActive(false);
        playerIcon.gameObject.SetActive(false);
    }

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
        playerIcon.gameObject.SetActive(isMapActive);

        // Optionally pause the game
        Time.timeScale = isMapActive ? 0 : 1;
    }
}