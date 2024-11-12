using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TerrainPath : MonoBehaviour
{
    public TerrainPathNode[] nodes;
	private bool isLoopPath = false;
	private void Start()
	{
		nodes = GetComponentsInChildren<TerrainPathNode>();
		isLoopPath = VerifyIfLoop();
	}

	public TerrainPathNode GetNodeCloseToTransform(Vector2 agent)
	{
		TerrainPathNode result = nodes
			.Select(node => (Vector2.Distance(node.transform.position, agent), node))
			.OrderBy(tuple => tuple.Item1)
			.ToList()
			.First().node;

		return result;
	}

	public bool IsLoopPath()
	{
		return isLoopPath;
	}

	private bool VerifyIfLoop()
	{
		bool firstNodeLoop = false;
		if (nodes[0].previous && nodes[0].previous.id == nodes[^1].id)
		{
			firstNodeLoop = true;
		}

		bool lastNodeLoop = false;
		if(nodes[^1].next && nodes[^1].next.id == nodes[0].id)
		{
			lastNodeLoop = true;
		}

		if(firstNodeLoop != lastNodeLoop)
		{
			throw new System.NullReferenceException($"Um dos nodes está para ser loop e o outro não, no Terrain Path: {gameObject.name}");
		}

		return firstNodeLoop;
	}
}