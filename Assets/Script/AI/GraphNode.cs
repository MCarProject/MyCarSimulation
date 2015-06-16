using UnityEngine;
using System.Collections.Generic;

public class GraphNode : MonoBehaviour {
	public int index;
	public List<GraphNode> connectedList = null;
	public float gCost = Mathf.Infinity;
	public float hCost = Mathf.Infinity;

	public GraphNode parent;

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (this.transform.position, 0.2f);
		for(int i=0; i<connectedList.Count; i++) {
			Gizmos.DrawLine (this.transform.position, connectedList[i].transform.position);
		}
	}
}
