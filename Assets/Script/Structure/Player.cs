using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : CarBase {
	public static float playerVelocity;
	public static float playerRPM;
	public static float playerDurability;
	public static CarState playerCarState;
	public static bool playerClutch;
	Text debugUI;

	void Start () {
		std_torque = 300.0f;
		std_maximumVelocity = 30.0f;
		maximumTurnAngle = 30.0f;

		carState = CarState.GEARS_N;
		Initialize ();

		debugUI = GameObject.Find ("Debug").GetComponent<Text> ();
	}

	void Update () {
		UpdateRPM ();
		UpdateWheelTransform ();
		Accelerate();
		Brake_Strong ();
		Turn ();
		debugUI.text = ("v:" + rigidBody.velocity.magnitude + "/" + std_maximumVelocity * gearFactor + 
		                ", a:" + wheelCollider[0].motorTorque + ", g:" + gearFactor + 
		                ", S:" + carState + ", RPM:" + RPM);
		SetPlayerStatus ();
	}

	void SetPlayerStatus() {
		playerVelocity = rigidBody.velocity.magnitude;
		playerRPM = RPM;
		playerDurability = durability;
		playerCarState = carState;
		playerClutch = isClutched;
	}
}
