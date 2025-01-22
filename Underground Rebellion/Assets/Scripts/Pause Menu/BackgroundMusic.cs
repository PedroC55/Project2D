using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
     public AudioSource m_MyAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager instance is null in PauseSettingsMenu. Ensure it exists in the first scene.");
            return;
        }
        m_MyAudioSource.volume = SettingsManager.Instance.MasterVolume * SettingsManager.Instance.MusicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        m_MyAudioSource.volume = SettingsManager.Instance.MasterVolume * SettingsManager.Instance.MusicVolume;
    }
}
