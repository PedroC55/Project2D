using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	private Interaction checkpointInteraction;

	// Start is called before the first frame update
	void Start()
    {
		checkpointInteraction = GetComponent<Interaction>();
		checkpointInteraction.OnInteraction += SaveGame;
	}

	private void SaveGame()
	{
		LevelEvent.PlayerSave(transform);
	}
}
