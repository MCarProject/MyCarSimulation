using UnityEngine;
using System.Collections;

public class Player : CarBase {

	void Start () {
		RPM = 0.0f;
		maximumVelocity = 25.0f;
		limitedVelocity = 0.0f;
		velocity = 0.0f;
		maximumAcceleration = 1.0f;
		limitedAcceleration = 0.0f;
		acceleration = 0.0f;
		accelerationFactor = maximumAcceleration;
		gearValue = 0.0f;
		gearFactor = 0.0f;
		collisionFactor = 0.2f;
		earthingFactor = 0.6f;
		turnRate = 50.0f;
		durability = 1.0f;
		carState = CarState.GEARS_N;
		
		rigidbody = this.GetComponent<Rigidbody> ();
		SetGear ();
	}

	void Update () {
		Movement ();
		Debug.Log ("v:" + velocity + "/" + limitedVelocity + ", a:" + acceleration +
		           "/" + limitedAcceleration + ", g:" + gearFactor + ", S:" + carState +
		           ", RPM:" + RPM);
	}
}
