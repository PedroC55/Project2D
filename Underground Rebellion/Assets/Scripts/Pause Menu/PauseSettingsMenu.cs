using UnityEngine;
using UnityEngine.UI;

public class PauseSettingsMenu : MonoBehaviour
{
    // References to the sliders in the UI
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        if (SettingsManager.Instance == null || SettingsManager.Instance.gameObject == null)
        {
            Debug.LogError("SettingsManager.Instance is null or destroyed.");
        }
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager instance is null in PauseSettingsMenu. Ensure it exists in the first scene.");
            return;
        }

        Debug.Log("Entrei no if");
        masterVolumeSlider.value = SettingsManager.Instance.MasterVolume;
        musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;
        sfxVolumeSlider.value = SettingsManager.Instance.SFXVolume;

        Debug.Log("Master na pausa " + masterVolumeSlider.value);
        

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