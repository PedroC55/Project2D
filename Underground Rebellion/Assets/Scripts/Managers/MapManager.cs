using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject mapUI;
    public Transform playerIcon;

	[Serializable]
	private class RoomImage
	{
		public int ID;

		public Image MapImage;
	}
	[SerializeField] private List<RoomImage> rooms = new();
    private Dictionary<int,  Image> roomImages = new();

    public static bool isMapActive = false;

	private void OnEnable()
	{
        CanvasEvent.OnUpdateMap += UpdateMap;
	}

	private void OnDisable()
	{
		CanvasEvent.OnUpdateMap -= UpdateMap;
	}

	private void Start()
    {
        mapUI.SetActive(false);

        foreach(RoomImage roomImage in rooms)
        {
            roomImages[roomImage.ID] = roomImage.MapImage;
        }

        rooms.Clear();
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

        Time.timeScale = isMapActive ? 0 : 1;
    }

    private void UpdateMap(int roomID)
    {
        var tempColor = roomImages[roomID].color;
        tempColor.a = 1f;

		roomImages[roomID].color = tempColor;
        playerIcon.position = roomImages[roomID].transform.position;
	}
}