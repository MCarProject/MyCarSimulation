using UnityEngine;

public class GraphEdge {
	public GraphNode from;
	public GraphNode to;
	public float distance;

	public GraphEdge(GraphNode from, GraphNode to) {
		this.from = from;
		this.to = to;

		this.distance = (from.transform.position - to.transform.position).magnitude;
	}
}
