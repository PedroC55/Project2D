using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPCController : MonoBehaviour
{
	[Serializable]
	public class NPCPositionsData
	{
		public string name;
		public Transform transform;
	}

	[SerializeField] private List<NPCPositionsData> npcPositionsData;
	private Dictionary<string, Transform> npcPositionsDictionary = new();
	
	private Agent agent;
	private Animator animator;
	private Collider2D col2D;
	private Rigidbody2D rb2D;

	private Transform positionToMoveTo;

	void Awake()
	{

		foreach(var position in npcPositionsData)
		{
			npcPositionsDictionary.Add(position.name, position.transform);
		}

		agent = GetComponent<Agent>();
		col2D = GetComponent<Collider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
	}

	void Update()
	{
		if (positionToMoveTo == null)
			return;

		float distance = Vector2.Distance(positionToMoveTo.position, transform.position);

		if (distance <= 0.2f)
		{
			positionToMoveTo = null;
			agent.MovementInput = Vector2.zero;
			Disapear();
			return;
		}

		Vector2 direction = (positionToMoveTo.position - transform.position).normalized;
		agent.MovementInput = direction;
	}

	[YarnCommand("move_to")]
	public void SetMovementInput(string positionName)
	{
		positionToMoveTo = npcPositionsDictionary[positionName];
	}

	[YarnCommand("face_direction")]
	public void FaceDirection(string direction)
	{
		direction = direction.ToLower();
		if (direction == "left")
		{
			agent.FaceDirection(Vector2.left);
		}
		else if (direction == "right")
		{
			agent.FaceDirection(Vector2.right);
		}
	}

	[YarnCommand("set_position")]
	public void SetPosition(string positionName)
	{
		agent.ResetAgent(false);
		col2D.isTrigger = false;

		transform.position = npcPositionsDictionary[positionName].position;
		//Reinicia o animator
		animator.Rebind();
		animator.Update(0f);
	}

	private void Disapear()
	{
		agent.Disappear();
		col2D.isTrigger = true;
	}
}
