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
	
	//Sound
	AudioSource[] soundEffect;
	AudioSource engineEffect;

	//Light
	public static Light[] headLight;

	void Start () {
		std_torque = 300.0f;
		std_maximumVelocity = 30.0f;
		maximumTurnAngle = 30.0f;
		

		carState = CarState.GEARS_N;
		Initialize ();

		debugUI = GameObject.Find ("Debug").GetComponent<Text> ();

		PlayerInit ();

		soundEffect = GetComponentsInChildren<AudioSource> ();
		foreach (AudioSource audio in soundEffect) {
			if(audio.name == "CarEngineEffect") {
				engineEffect = audio;
				engineEffect.volume = GlobalConfig.worldEffect * 0.3f;
			}
		}
		headLight = GetComponentsInChildren<Light> ();
	}

	void PlayerInit () {
		std_torque = GlobalConfig.playerAcceleration;
		std_maximumVelocity = GlobalConfig.playerSpeed;

		Vector3 tempCenterOfMass = rigidBody.centerOfMass;
		tempCenterOfMass.y -= GlobalConfig.playerStability;
		rigidBody.centerOfMass = tempCenterOfMass;
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

		OnEngineSound ();
	}

	void SetPlayerStatus() {
		playerVelocity = rigidBody.velocity.magnitude;
		playerRPM = RPM;
		playerDurability = durability;
		playerCarState = carState;
		playerClutch = isClutched;
	}
	
	void OnEngineSound() {
		if (carState == CarState.ENGINE_OFF) {
			engineEffect.Stop ();
		} else {
			if(!engineEffect.isPlaying) 
				engineEffect.Play();
		}
		engineEffect.pitch = (RPM * 2.333333f) + 0.166666f;

		if (engineEffect.pitch < 0.4f)
			engineEffect.pitch = 0.4f;
		else if (engineEffect.pitch > 2.4f)
			engineEffect.pitch = 2.4f;
	}
}
