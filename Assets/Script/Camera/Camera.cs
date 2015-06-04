using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	const float minimumDistance = 5.0f;
	const float maximumDistance = 9.0f;

	GameObject target;
	float trackingDistance;	//normalize 사용!!
	float mouseSensitivity;
	
	Vector3 offset;

	void Start () {
		target = GameObject.Find ("Player");
		trackingDistance = 7.0f;
		mouseSensitivity = 2.0f;

		offset = new Vector3(-trackingDistance, trackingDistance, -trackingDistance);
	}
	void Update () {
	}
	void LateUpdate() {
		SetCameraPosition ();
		SetCameraDistance ();
	}

	void SetCameraPosition() {
		offset.Normalize ();
		Vector3 tempPosition = target.transform.position + (offset * trackingDistance);

		this.transform.position = tempPosition;

		if (Cursor.lockState == CursorLockMode.None) { return; }

		this.transform.RotateAround (target.transform.position, Vector3.up, Input.GetAxis ("Mouse X")*mouseSensitivity);

		float temp = this.transform.rotation.eulerAngles.x;
		if (temp > 70 && Input.GetAxis ("Mouse Y") > 0) {
		} else if (temp < 20 && Input.GetAxis ("Mouse Y") < 0) {
		} else {
			this.transform.Translate (new Vector3 (0, Input.GetAxis ("Mouse Y")*0.3f, 0));
		}

		this.transform.LookAt (target.transform.position);
		
		offset = this.transform.position - target.transform.position ;
	}
	void SetCameraDistance() {
		if (Input.GetKey (KeyCode.KeypadPlus)) {
			trackingDistance += Time.deltaTime * 2.0f;
		}
		if (Input.GetKey (KeyCode.KeypadMinus)) {
			trackingDistance -= Time.deltaTime * 2.0f;
		}
		if (trackingDistance > maximumDistance) {
			trackingDistance = maximumDistance;
		} else if (trackingDistance < minimumDistance) {
			trackingDistance = minimumDistance;
		}
	}
}
