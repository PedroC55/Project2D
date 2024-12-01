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
    PARRING_1,
    PARRING_2, 
    PARRING_3,
    FINAL_PARRING,
    KILL,
    DAMAGE,
    DEATH,
    BREAK,
    LEVER,
    PLATFORM_MOVING,
    SURVIVOR,
    SPIKE,
    ENEMY_ALERT,
    ENEMY_FORGETS
}

[RequireComponent(typeof(AudioSource))]

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
