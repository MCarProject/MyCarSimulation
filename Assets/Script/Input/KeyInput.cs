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

		player.Turn(Input.GetAxis("Horizontal"));
	}
}
