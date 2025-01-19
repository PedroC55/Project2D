using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredresolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        filteredresolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredresolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredresolutions.Count; i++)
        {
            string resolutionOption = filteredresolutions[i].width + "x" + filteredresolutions[i].height + " " + filteredresolutions[i].refreshRateRatio.value;
            options.Add(resolutionOption);
            if (filteredresolutions[i].width == Screen.width && filteredresolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredresolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }


}
