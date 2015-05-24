using UnityEngine;
using System.Collections;

public class WorldInfo : MonoBehaviour {
	static WorldInfo worldInfo;
	const int indexMaximumRow = 2;
	const int indexMaximumcol = 3;
	RoadInfo[,] roadInfo = new RoadInfo[indexMaximumRow, indexMaximumcol];

	// Use this for initialization
	void Start () {
		worldInfo = this;
		worldInfo.SetWorldInfo ();
	}

	private void SetWorldInfo() {
		for (int x = 0; x < indexMaximumRow; x++) {
			for(int y = 0; y < indexMaximumcol; y++) {
				for(int z = 0; z < 2; z++) {
					if((x == (indexMaximumRow-1) && z == 0) || (y == (indexMaximumcol-1) && z == 1)) {
						continue;
					} else {
						roadInfo[x, y] = new RoadInfo();
						roadInfo[x, y].SetRoadInfo(new Vector3(x, y, z));
					}
				}
			}
		}
		Debug.Log ("Init world");
	}
}
