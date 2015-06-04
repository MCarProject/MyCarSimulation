using UnityEngine;
using System.Collections;

public class KeyInput : MonoBehaviour {
	CarBase player;

	void Start () {
		player = GameObject.Find ("Player").GetComponent<CarBase> ();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void LateUpdate() {
	}

	void Update () {
		GetKeyInput ();
	}

	void GetKeyInput() {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			if(Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			} else {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
		if (Input.GetKey (KeyCode.Space)) {
			player.OnBrake (true);
		} else if (Input.GetKeyUp (KeyCode.Space)) {
			player.OnBrake (false);
		}
		if(Input.GetKeyDown (KeyCode.UpArrow)) {
			player.ChangeGear(false);
		} else if(Input.GetKeyDown (KeyCode.DownArrow)) {
			player.ChangeGear(true);
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			player.ChangeGear(true);
		} else if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			player.ChangeGear(false);
		}
		if(Input.GetKeyDown(KeyCode.R)) {
			player.EngineControl();
		}
		if(Input.GetKeyDown(KeyCode.C)) {
			player.OnClutched(true);
		} else if(Input.GetKeyUp(KeyCode.C)) {
			player.OnClutched(false);
		}
	}
}
