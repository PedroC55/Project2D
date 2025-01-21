using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    private void OnEnable()
    {
        HitEvent.OnHit += HandleHit;
    }

    private void OnDisable()
    {
        HitEvent.OnHit -= HandleHit;
    }

    private void HandleHit(int damage, GameObject sender, GameObject receiver)
    {
        // Check if the receiver of the hit is this object
        if (sender.CompareTag("Player") && receiver.GetInstanceID() == gameObject.GetInstanceID())
        {
            // Make the object disappear
            gameObject.SetActive(false);
            // Play sound effect (if applicable)
            SoundManager.Instance.PlaySound(SoundType.BREAK);
        }
    }
}
