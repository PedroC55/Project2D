using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundExit : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layeredIndex)
    {
        SoundManager.Instance.PlaySound(sound);
    }
}
