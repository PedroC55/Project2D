using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorInteraction : MonoBehaviour
{
	private Interaction survivorInteraction;
	private bool elevatorFixed = false;

	private void OnDisable()
	{
		survivorInteraction.OnInteraction -= FixElevator;
	}

	private void Start()
	{
		survivorInteraction = GetComponent<Interaction>();
		survivorInteraction.OnInteraction += FixElevator;
	}

    private void FixElevator()
    {
		SoundManager.Instance.PlaySound(SoundType.SURVIVOR);
		DialogueManager.Instance.StartNode("Survivor");
		
		if (!elevatorFixed)
		{
			LevelEvent.FixElevator();
			elevatorFixed = true;
		}
    }
}