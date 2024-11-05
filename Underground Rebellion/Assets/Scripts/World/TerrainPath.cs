using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TerrainPath : MonoBehaviour
{
    public TerrainPathNode[] nodes;

	private void Start()
	{
		nodes = GetComponentsInChildren<TerrainPathNode>();
	}

	public TerrainPathNode GetNodeCloseToTransform(Transform agent)
	{
		TerrainPathNode result = nodes
			.Select(node => (Vector2.Distance(node.transform.position, agent.position), node))
			.OrderBy(tuple => tuple.Item1)
			.ToList()
			.First().node;

		return result;
	}
}