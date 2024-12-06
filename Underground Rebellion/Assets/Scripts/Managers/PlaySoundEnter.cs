using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layeredIndex)
    {
        SoundManager.PlaySound(sound);
    }
}
