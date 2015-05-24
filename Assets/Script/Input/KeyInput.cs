using UnityEngine;
using System.Collections;

public class KeyInput : MonoBehaviour {
	CarBase player;

	void Start () {
		player = GameObject.Find ("Player").GetComponent<CarBase> ();
	}

	void LateUpdate() {
	}

	void Update () {
		GetKeyInput ();
	}

	void GetKeyInput() {
		if(Input.GetKey(KeyCode.W)) {
			player.Accelerate();
		} else if(Input.GetKey(KeyCode.S)) {
			player.Reverse ();
		}
		if(Input.GetKey(KeyCode.Space)) {
			player.Brake ();
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

		player.Turn(Input.GetAxis("Horizontal"));
	}
}
