using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : CarBase {
	Text debugUI;

	void Start () {
		RPM = 0.0f;
		maximumVelocity = 25.0f;
		limitedVelocity = 0.0f;
		velocity = 0.0f;
		maximumReverseVelocity = maximumVelocity * 0.4f;
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

		debugUI = GameObject.Find ("Debug").GetComponent<Text> ();
	}

	void Update () {
		Movement ();
		//Debug.Log ("v:" + velocity + "/" + limitedVelocity + ", a:" + acceleration
		//           + ", g:" + gearFactor + ", S:" + carState +
		//           ", RPM:" + RPM);
		debugUI.text = ("v:" + velocity + "/" + limitedVelocity + 
		                ", rV:" + reverseVelocty + "/" + maximumReverseVelocity + 
		                ", a:" + acceleration + ", g:" + gearFactor + 
		                ", S:" + carState + ", RPM:" + RPM);
	}
}
