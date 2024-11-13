using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPathNode : MonoBehaviour
{
	public int id;
	public TerrainPathNode previous;
	public TerrainPathNode next;
	
	private TerrainPath tPath;
	private SpriteRenderer spRenderer;

	private void Start()
	{
		tPath = GetComponentInParent<TerrainPath>();
		spRenderer = GetComponent<SpriteRenderer>();
	}

	public void PaintColor(Color color)
	{
		spRenderer.color = color;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Enemy"))
			return;

		if (!collision.GetComponent<WallMovement>())
			return;

		//Mudar para verificar no EnemyAi da formiga a propriedade tpath
		WallMovement enemyWM = collision.GetComponent<WallMovement>();
		if(enemyWM.tPath.GetInstanceID() == tPath.GetInstanceID())
		{
			enemyWM.UpdateNode(this);
		}
	}
}
