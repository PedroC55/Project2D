using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    public void PlaySound()
    {
        SoundManager.Instance.PlaySound(SoundType.WALK);
    }
}
