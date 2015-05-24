using UnityEngine;
using System.Collections;

public class AIBase : CarBase {

	void Start () {
		RPM = 0.0f;
		maximumVelocity = 25.0f;
		limitedVelocity = 0.0f;
		velocity = 0.0f;
		maximumReverseVelocity = 8.0f;
		reverseVelocty = 0.0f;
		standardAcceleration = 1.0f;
		acceleration = 0.0f;
		gearValue = 0.0f;
		gearFactor = 0.0f;
		collisionFactor = 0.2f;
		earthingFactor = 0.6f;
		turnRate = 50.0f;
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
