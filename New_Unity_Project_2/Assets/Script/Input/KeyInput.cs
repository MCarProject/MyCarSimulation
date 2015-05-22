using UnityEngine;
using System.Collections;

public class KeyInput : MonoBehaviour {
	Rigidbody rb;
	CarBase player;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		player = GameObject.Find ("User").GetComponent<CarBase> ();
	}

	void LateUpdate() {
	}

	// Update is called once per frame
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
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			player.OnGearChange(CarBase.CarState.GEARS_1);
		}else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			player.OnGearChange(CarBase.CarState.GEARS_2);
		}else if(Input.GetKeyDown(KeyCode.Alpha3)) {
			player.OnGearChange(CarBase.CarState.GEARS_3);
		}else if(Input.GetKeyDown(KeyCode.Alpha4)) {
			player.OnGearChange(CarBase.CarState.GEARS_4);
		}else if(Input.GetKeyDown(KeyCode.Alpha5)) {
			player.OnGearChange(CarBase.CarState.GEARS_5);
		}else if(Input.GetKeyDown(KeyCode.R)) {
			player.OnGearChange(CarBase.CarState.ENGINE_OFF);
		}else if(Input.GetKeyDown(KeyCode.T)) {
			player.OnGearChange(CarBase.CarState.GEARS_N);
		}else if(Input.GetKeyDown(KeyCode.C)) {
			player.OnGearChange(CarBase.CarState.CLUTCHED);
		}

		player.Turn(Input.GetAxis("Horizontal"));
	}
}
