using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
	public static CameraManager Instance { get; private set; }

	private CinemachineImpulseSource _impulseSource;

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

		_impulseSource = GetComponent<CinemachineImpulseSource>();
	}

	public void ShakeCamera(float shakeIntensity)
	{
		_impulseSource.GenerateImpulseWithForce(shakeIntensity);
	}
}
