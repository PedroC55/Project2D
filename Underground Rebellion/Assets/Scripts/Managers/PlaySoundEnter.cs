using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0,1)] private float volume = 1;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layeredIndex)
    {
        SoundManager.PlaySound(sound, volume);
    }
}
