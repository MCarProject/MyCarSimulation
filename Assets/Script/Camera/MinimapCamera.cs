using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour {
	Transform player;

	void Start () {
		player = GameObject.Find ("Player").GetComponent<Transform> ();
	}	

	void LateUpdate () {
		Vector3 tempPosition = player.position;
		tempPosition.y = 45.0f;
		Vector3 tempRotation = player.rotation.eulerAngles;
		tempRotation.x = 90.0f;
		tempRotation.z = 0.0f;

		this.transform.position = tempPosition;
		this.transform.rotation = Quaternion.Euler (tempRotation);
	}
}
