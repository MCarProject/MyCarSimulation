using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour {
	private static int nextNodeIndex = 0;
	GraphNode[] nodeSet = null;
	List<GraphEdge> edgeSet = new List<GraphEdge>();

	void Awake() {
		SetList ();
	}

	void SetList() {
		nodeSet = GetComponentsInChildren<GraphNode> ();
		for (int i=0; i<nodeSet.Length; i++) {
			nodeSet[i].index = nextNodeIndex;
			nextNodeIndex++;
		}
		for (int i=0; i<nodeSet.Length; i++) {
			for (int j=0; j<nodeSet[i].connectedList.Count; j++) {
				GraphEdge newEdge;
				newEdge = new GraphEdge(nodeSet[i], nodeSet[i].connectedList[j]);
				edgeSet.Add (newEdge);
			}
		}
	}

	public int GetMyNode(Vector3 myPosition) {
		float tempDistance = Mathf.Infinity;
		int nodeNumber = 0;
		foreach (GraphNode nodes in nodeSet) {
			float distance = (nodes.transform.position - myPosition).magnitude;
			if(tempDistance > distance) {
				tempDistance = distance;
				nodeNumber = nodes.index;
			}
		}
		return nodeNumber;
	}
	public int GetRandomNode() {
		int nodeCount = nodeSet.Length;
		int randomNumber = Random.Range(0, nodeCount);
		if (randomNumber == nodeCount)
			randomNumber--;
		return randomNumber;
	}

	public List<Transform> NodeToTransform(List<GraphNode> path) {
		List<Transform> newList = new List<Transform>();

		foreach (GraphNode nodes in path) {
			newList.Add(nodes.transform);
		}
		return newList;
	}
	public List<GraphNode> SearchPath(int startIndex, int targetIndex) {
		GraphNode start = null;
		GraphNode target = null;

		foreach (GraphNode nodes in nodeSet) {
			if(nodes.index == startIndex) {
				start = nodes;
			} else if(nodes.index == targetIndex) {
				target = nodes;
			}
		}
		return SearchPath (start, target);
	}
	public List<GraphNode> SearchPath(GraphNode start, GraphNode target) {
		List<GraphNode> openSet = new List<GraphNode>();
		List<GraphNode> closedSet = new List<GraphNode>();
		openSet.Add (start);
		while (openSet.Count > 0) {
			GraphNode currentNode = openSet[0];
			for(int i=1; i<openSet.Count; i++) {
				if(openSet[i].hCost + openSet[i].gCost < currentNode.hCost + currentNode.gCost ||
				   openSet[i].hCost + openSet[i].gCost == currentNode.hCost + currentNode.gCost && 
				   openSet[i].hCost < currentNode.hCost) {
					currentNode = openSet[i];
				}
			}
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
			if(currentNode.index == target.index) { 
				return RetracePath(start, target);
			}
			foreach (GraphNode neighbour in currentNode.connectedList) {
				if (closedSet.Contains(neighbour)) { continue; }
				float newMovementCostToNeighbour = currentNode.gCost + GetEdge(currentNode, neighbour).distance;
				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, target);
					neighbour.parent = currentNode;
					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
		return null;
	}
	List<GraphNode> RetracePath(GraphNode startNode, GraphNode endNode) {
		List<GraphNode> path = new List<GraphNode>();
		GraphNode currentNode = endNode;
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		if (currentNode == startNode) {
			path.Add(currentNode);
		}
		path.Reverse();
		return path;
	}
	
	GraphEdge GetEdge(GraphNode a, GraphNode b) {
		foreach (GraphEdge edge in edgeSet) {
			if(edge.from.index == a.index && edge.to.index == b.index) {
				return edge;
			}
		}
		return null;
	}
	float GetDistance(GraphNode a, GraphNode b) {
		float temp = (a.transform.position - b.transform.position).magnitude;
		return temp;
	}
}