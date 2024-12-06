using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    WALK,
    JUMP_1,
    JUMP_2,
    JUMP_3,
    LANDING,
    PARRY,
    HIT_DENIED, 
    MELEE,
    REMOVE_WEAPON,
    KILL,
    DAMAGE,
    DEATH,
    BREAK,
    LEVER,
    PLATFORM_MOVING,
    SURVIVOR,
    SPIKE,
    HOVER,
    SELECT
}

[RequireComponent(typeof(AudioSource))]

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    public static SoundManager Instance;
    private AudioSource audioSource;

    private void Awake()
    {
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		else if (Instance.gameObject == null)
		{
			// Reassign if the existing instance was destroyed
			Instance = this;
		}
	}

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType sound)
    {
		audioSource.PlayOneShot(Instance.soundList[(int)sound], SettingsManager.Instance.SFXVolume * SettingsManager.Instance.MasterVolume);
    }

	public void PlaySound(AudioClip audio)
	{
		audioSource.PlayOneShot(audio, SettingsManager.Instance.SFXVolume * SettingsManager.Instance.MasterVolume);
	}
}
