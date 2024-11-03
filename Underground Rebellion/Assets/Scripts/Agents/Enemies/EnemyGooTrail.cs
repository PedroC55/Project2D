using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGooTrail : MonoBehaviour
{
	public GameObject gooPrefab;
	public float gooDistanceRate = 0.5f;
	private Vector2 lastPosition;
	private float currentDistance = 0f;
	private Vector3 positionToInstantiate;

	private void Start()
	{
		lastPosition = transform.parent.position;
	}

	void Update()
	{
		Vector2 currentPosition = transform.parent.position;

		currentDistance += Vector2.Distance(lastPosition, currentPosition);

		lastPosition = currentPosition;

		if (currentDistance >= gooDistanceRate)
		{
			currentDistance = 0f;
			
			positionToInstantiate = transform.parent.position;
			positionToInstantiate.y += gooPrefab.transform.position.y;

			GameObject goo = Instantiate(gooPrefab, positionToInstantiate, Quaternion.identity);
			goo.transform.RotateAround(transform.parent.position, Vector3.forward, transform.rotation.eulerAngles.z);
		}

	}
}
