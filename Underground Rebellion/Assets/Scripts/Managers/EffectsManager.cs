using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
	public static EffectsManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void PlayOneShot(ParticleSystem vfx, Vector2 position, Vector2 direction)
    {
		ParticleSystem particleInstance = Instantiate(vfx, position, Quaternion.identity);

		var velocityModule = particleInstance.velocityOverLifetime;
		velocityModule.x = direction.x;
		velocityModule.y = direction.y;

		particleInstance.Play();
		Destroy(particleInstance.gameObject, particleInstance.main.duration + particleInstance.main.startLifetime.constantMax);
	}

}
