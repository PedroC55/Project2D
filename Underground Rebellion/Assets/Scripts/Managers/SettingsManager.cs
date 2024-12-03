using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public float MasterVolume = 1f;
    public float MusicVolume = 1f;
    public float SFXVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the object between scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
}