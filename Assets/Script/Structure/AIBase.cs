using UnityEngine;
using System.Collections;

public class AIBase : CarBase {

	void Start () {
		RPM = 0.0f;
		gearFactor = 0.0f;
		durability = 1.0f;
		carState = CarState.GEARS_N;
		
		rigidBody = this.GetComponent<Rigidbody> ();
	}

	void Update () {
	}

	protected void SearchDestination() {

	}

	protected void SearchPath() {

	}
}
