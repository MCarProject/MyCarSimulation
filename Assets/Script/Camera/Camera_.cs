using UnityEngine;
using System.Collections;

public class Camera_ : MonoBehaviour {
	const float minimumDistance = 5.0f;
	const float maximumDistance = 9.0f;

	GameObject target;
	GameObject viewPoint;
	float trackingDistance;	//normalize 사용!!
	float mouseSensitivity;

	bool isFirstPersonView = false;
	Vector3 firstPersonRotation = new Vector3 (0, 0, 0);
	
	Vector3 offset;

	void Start () {
		target = GameObject.Find ("Player");
		viewPoint = GameObject.Find ("ViewPoint");
		trackingDistance = 7.0f;
		mouseSensitivity = 2.0f;

		offset = new Vector3(-trackingDistance, trackingDistance, -trackingDistance);
	}
	void Update () {
		SetViewPerson ();
		//Debug.Log (Cursor.lockState + ", " + Cursor.visible);
	}
	void LateUpdate() {
		if (isFirstPersonView) {
			SetFirstCameraPosition();
			SetFirstCameraRotation();
		} else {
			SetCameraPosition ();
			SetCameraDistance ();
		}
	}

	void SetViewPerson() {
		if (Input.GetKeyDown (KeyCode.V)) {
			isFirstPersonView = !isFirstPersonView;

			if(isFirstPersonView) {
				Camera.main.fieldOfView = 80.0f;
			}
			else {
				Camera.main.fieldOfView = 60.0f;
			}
		}
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

	void SetFirstCameraPosition() {
		this.transform.position = viewPoint.transform.position;
	}
	void SetFirstCameraRotation() {
		firstPersonRotation.y += Input.GetAxis ("Mouse X") * 1.5f;
		if (firstPersonRotation.y > 50.0f) {
			firstPersonRotation.y = 50.0f;
		} else if(firstPersonRotation.y < -50.0f) {
			firstPersonRotation.y = -50.0f;
		}

		Vector3 tempRotation = target.transform.rotation.eulerAngles;
		tempRotation += firstPersonRotation;

		this.transform.rotation = Quaternion.Euler (tempRotation);
	}
}
