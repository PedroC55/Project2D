using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // References to the sliders in the UI
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        // Initialize sliders with current settings
        if (SettingsManager.Instance != null)
        {
            masterVolumeSlider.value = SettingsManager.Instance.MasterVolume;
            musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;
            sfxVolumeSlider.value = SettingsManager.Instance.SFXVolume;

            Debug.Log("Master nos menus: " + masterVolumeSlider.value);
        }

        // Add listeners to sliders to handle value changes
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.MasterVolume = volume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.MusicVolume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SFXVolume = volume;
        }
    }
}