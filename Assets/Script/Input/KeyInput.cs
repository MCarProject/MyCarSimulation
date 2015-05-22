using UnityEngine;
using System.Collections;

public class KeyInput : MonoBehaviour {
	float engineCount;
	Rigidbody rb;
	CarBase player;

	void Start () {
		engineCount = 0.0f;
		rb = GetComponent<Rigidbody> ();
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
		}
		if(Input.GetKey(KeyCode.S)) {
			player.Decelerate();
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
