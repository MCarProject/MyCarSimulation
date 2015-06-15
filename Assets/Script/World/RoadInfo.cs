using UnityEngine;
using System.Collections;

public class RoadInfo {
	public Vector3 index;	// x, y: startPosition, z: direction
	public Vector3 startPosition;
	public Vector3 endPosition;

	public void SetRoadInfo (Vector3 index) {
		this.index = index;
		startPosition = GameObject.Find ("Marker: " + index.x + ", " + index.y).transform.position;

		if (index.z == 0) {
			endPosition = GameObject.Find ("Marker: " + (index.x + 1) + ", " + index.y).transform.position;
		} else {
			endPosition = GameObject.Find ("Marker: " + index.x + ", " + (index.y + 1)).transform.position;
		}

		Debug.Log ("index: "+this.index + " :: sPos: " + startPosition + ", ePos: " + endPosition);
	}

	void Start () {
	
	}

	void Update () {
	
	}
}
