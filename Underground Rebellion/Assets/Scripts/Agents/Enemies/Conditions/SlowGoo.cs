using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGoo : MonoBehaviour
{
	public int slowPercentage = 50;

	[SerializeField]
	public float lifeTime = 3f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}
}
