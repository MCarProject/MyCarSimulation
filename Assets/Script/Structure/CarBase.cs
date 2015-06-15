using UnityEngine;
using System.Collections;

public class CarBase : MonoBehaviour {
	public enum CarState { ENGINE_OFF, GEARS_N, GEARS_R, GEARS_1, GEARS_2, 
		GEARS_3, GEARS_4, GEARS_5 };

	protected float std_torque = 200.0f;					//기준토크
	protected float std_maximumVelocity = 20.0f;			//기준최대속력
	protected float maximumTurnAngle = 20.0f;				//최대회전각
	protected float RPM = 0.0f;								//엔진회전속도

	protected bool onBrake = false;
	protected bool isClutched = false;						//클러치상태
	private bool isDownGear = false;						//기어조작상태

	protected float gearFactor = 0.0f;						//기어계수
	protected float durability = 1.0f;						//내구도
	protected CarState carState = CarState.GEARS_1;			//기어상태
	
	protected Rigidbody rigidBody;

	public WheelCollider[] wheelCollider = new WheelCollider[4];
	public Transform[] wheelTransform = new Transform[4];
	public Transform handleTransform;

	/* Initialization for rigidbody and center of mass*/
	protected void Initialize() {
		rigidBody = this.GetComponent<Rigidbody> ();
		Vector3 tempCenterOfMass = rigidBody.centerOfMass;
		tempCenterOfMass.y -= 0.2f;
		rigidBody.centerOfMass = tempCenterOfMass;
	}

	/* Engine control with Input */
	public void OnClutched(bool on) {
		if (on) {
			isClutched = true;
			Brake_Weak();
		} else {
			isClutched = false;
		}
	}
	public void ChangeGear(bool state) {
		isDownGear = state;
		if (carState == CarState.ENGINE_OFF) {
			return;
		}
		if (!isClutched) {
			carState = CarState.ENGINE_OFF;
			Brake_Weak();
			gearFactor = 0.0f;
		} else {
			if (!isDownGear) {
				if (carState == CarState.GEARS_R) {
					carState = CarState.GEARS_N;
					gearFactor = 0.0f;
				} else if (carState == CarState.GEARS_N) {
					carState = CarState.GEARS_1;
					gearFactor = 0.4f;
				} else if (carState == CarState.GEARS_1) {
					carState = CarState.GEARS_2;
					gearFactor = 0.55f;
				} else if (carState == CarState.GEARS_2) {
					carState = CarState.GEARS_3;
					gearFactor = 0.7f;
				} else if (carState == CarState.GEARS_3) {
					carState = CarState.GEARS_4;
					gearFactor = 0.85f;
				} else if (carState == CarState.GEARS_4) {
					carState = CarState.GEARS_5;
					gearFactor = 1.0f;
				}
			} else {
				if (carState == CarState.GEARS_5) {
					carState = CarState.GEARS_4;
					gearFactor = 0.85f;
				} else if (carState == CarState.GEARS_4) {
					carState = CarState.GEARS_3;
					gearFactor = 0.7f;
				} else if (carState == CarState.GEARS_3) {
					carState = CarState.GEARS_2;
					gearFactor = 0.55f;
				} else if (carState == CarState.GEARS_2) {
					carState = CarState.GEARS_1;
					gearFactor = 0.4f;
				} else if (carState == CarState.GEARS_1) {
					carState = CarState.GEARS_N;
					gearFactor = 0.0f;
				} else if (carState == CarState.GEARS_N) {
					carState = CarState.GEARS_R;
					gearFactor = 0.4f;
				}
			}
		}
	}
	public void EngineControl() {
		if (carState == CarState.ENGINE_OFF) {
			carState = CarState.GEARS_N;
			gearFactor = 0.0f;
			SoundManager.StartingEffectManager(true);
		} else {
			carState = CarState.ENGINE_OFF;
			gearFactor = 0.0f;
			Brake_Weak();
		}
	}

	/* Update wheel transform */
	public void UpdateWheelTransform() {
		for(int i=0; i<4; i++) {
			if(wheelCollider[i].rpm > 0) {
				wheelTransform[i].Rotate(new Vector3(0,0,-rigidBody.velocity.magnitude * 3.14f));
			} else {
				wheelTransform[i].Rotate(new Vector3(0,0,rigidBody.velocity.magnitude * 3.14f));
			}
		}
	}

	/* Car control with Input (Player.cs) */
	protected void Accelerate() {
		if (carState == CarState.ENGINE_OFF) { return; }
		float maximumVelocity = std_maximumVelocity * gearFactor;
		if (Input.GetAxis ("Vertical") < 0) {
			if (carState == CarState.GEARS_R) {
				if(isClutched) { return; }
				for (int i=0; i<4; i++) {
					wheelCollider [i].motorTorque = std_torque * Input.GetAxis ("Vertical") * 0.6f;
				}
			} else {
				Brake_Weak();
				return;
			}
		} else {
			if (carState == CarState.GEARS_N || isClutched) { return; }
			if(carState == CarState.GEARS_R) {
				Brake_Weak();
				return;
			}
			if (rigidBody.velocity.magnitude < maximumVelocity) {
				for (int i=0; i<4; i++) {
					wheelCollider [i].motorTorque = (std_torque - (std_torque * (gearFactor - 0.4f))) * Input.GetAxis ("Vertical");
					if(!onBrake){
						wheelCollider [i].brakeTorque = 0.0f;
					}
				}
			} else {
				for (int i=0; i<4; i++) {
					wheelCollider [i].motorTorque = 0.0f;
					wheelCollider [i].brakeTorque = (std_torque - (std_torque * (gearFactor - 0.4f)));
				}
			}
		}
	}
	protected void Brake_Weak () {
		for (int i=0; i<4; i++) {
			wheelCollider [i].motorTorque = 0.0f;
			wheelCollider [i].brakeTorque = std_torque;
		}
	}
	protected void Brake_Strong() {
		if (onBrake) {
			for (int i=0; i<4; i++) {
				wheelCollider [i].motorTorque = 0.0f;
				wheelCollider [i].brakeTorque = std_torque * 3.0f;
			}
		} else {
			for (int i=0; i<4; i++) {
				wheelCollider [i].brakeTorque = 0.0f;
			}
		}
	}
	protected void Turn() {
		for (int i=0; i<2; i++) {
			Vector3 tempRotation = wheelTransform[i].rotation.eulerAngles;
			tempRotation.y = 270 + maximumTurnAngle * Input.GetAxis("Horizontal");
			wheelCollider[i].steerAngle = maximumTurnAngle * Input.GetAxis("Horizontal");
			tempRotation += this.transform.rotation.eulerAngles;
			wheelTransform[i].rotation = Quaternion.Euler(tempRotation);
		}
		Vector3 tempRotaion = new Vector3 (18.94f, 0, -120 * Input.GetAxis ("Horizontal"));
		tempRotaion += this.transform.rotation.eulerAngles;
		handleTransform.rotation = Quaternion.Euler (tempRotaion);
	}
	public void OnBrake(bool on) {
		if (on) {
			onBrake = true;
		}
		else {
			onBrake = false;
		}
	}

	/* Update RPM */
	protected void UpdateRPM() {
		RPM = rigidBody.velocity.magnitude / (std_maximumVelocity * gearFactor);
		if (RPM < 0.1f) {
			RPM = 0.1f;
		}
		if (carState == CarState.ENGINE_OFF) {
			RPM = 0.0f;
		} else  if (carState == CarState.GEARS_N) {
			RPM = 0.1f;
		}
	}
}
