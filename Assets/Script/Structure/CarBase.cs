using UnityEngine;
using System.Collections;

public class CarBase : ObjectBase {
	public enum CarState { ENGINE_OFF, GEARS_N, GEARS_R, GEARS_1, GEARS_2, 
					GEARS_3, GEARS_4, GEARS_5, CLUTCHED };

	protected float RPM;					//엔진회전속도	(0~1)
	protected float maximumVelocity;		//최대속도		(15~30 /s)
	protected float limitedVelocity;		//한계속도
	protected float velocity;				//속도
	protected float maximumAcceleration;	//최대가속도
	protected float limitedAcceleration;	//한계가속도
	protected float acceleration;			//가속도
	protected float accelerationFactor;		//가속계수
	protected float gearValue;				//기어수치
	protected float gearFactor;				//기어계수
	protected float collisionFactor;		//충돌계수	(0.2~0.6)
	protected float earthingFactor;			//접지계수	(0.5~0.9)
	protected float turnRate;				//회전율		(40~60 degree/s)
	protected float durability;				//내구도
	protected CarState carState;			//기어상태

	protected Rigidbody rigidbody;

	void Start () {
	}
	void Update() {
	}
	
/* Gear Processing */
	public void StartEngine() { 
		if (carState == CarState.ENGINE_OFF) {
			carState = CarState.GEARS_N;
		}
		SetGear ();
	}
	public void OffEngine() {
		carState = CarState.ENGINE_OFF;
		SetGear ();
	}
	public void OnClutch() {
		if (carState == CarState.ENGINE_OFF) {
			return;
		} else {
			carState = CarState.CLUTCHED;
		}
		SetGear ();
	}
	public void OnGearChange(CarState newCarState) {
		if (newCarState == CarState.ENGINE_OFF) {
			OffEngine ();
		} else if (newCarState == CarState.GEARS_N) {
			if(carState == CarState.CLUTCHED) {
				carState = newCarState;
			} else {
				StartEngine ();
			}
		} else if (newCarState == CarState.CLUTCHED) {
			OnClutch ();
		} else {
			if(carState != CarState.CLUTCHED) {
				OffEngine();
			}
			else {
				carState = newCarState;
			}
		}
		SetGear ();
	}
	protected void SetGear() {
		if (carState == CarState.ENGINE_OFF ||
			carState == CarState.GEARS_R ||
			carState == CarState.CLUTCHED) {
			gearFactor = 0.0f;
			return;
		} else if (carState == CarState.GEARS_N) {
			gearFactor = 0.1f;
		} else if (carState == CarState.GEARS_1) {
			gearFactor = 0.4f;
		} else if (carState == CarState.GEARS_2) {
			gearFactor = 0.55f;
		} else if (carState == CarState.GEARS_3) {
			gearFactor = 0.7f;
		} else if (carState == CarState.GEARS_4) {
			gearFactor = 0.85f;
		} else if (carState == CarState.GEARS_5) {
			gearFactor = 1.0f;
		}
	}
	protected void GearUpdate() {
		if (gearValue > gearFactor) {
			gearValue -= 0.05f * Time.deltaTime;
		} else if (gearValue < gearFactor) {
			gearValue += 0.05f * Time.deltaTime;
		}
		limitedVelocity = gearValue * maximumVelocity;
		limitedAcceleration = (1 / gearFactor) * maximumAcceleration;
		if (carState == CarState.GEARS_1) {
			gearValue = 0.4f;
		}
	}

/* Movement Processing */
	public void Accelerate() {
		if (carState == CarState.ENGINE_OFF ||
		    carState == CarState.GEARS_N ||
		    carState == CarState.CLUTCHED) {
			return;
		}
		if (acceleration < limitedAcceleration) {
			acceleration += 5.0f * Time.deltaTime;
		}
		checkValid ();
	}
	public void Decelerate() {
		acceleration = 0;
		velocity -= accelerationFactor * 5.0f * Time.deltaTime;
		checkValid ();
	}
	public void Turn(float axis) {
		if (velocity == 0) {
			return;
		}
		this.transform.Rotate (0, (axis * turnRate * Time.deltaTime), 0);
	}
	protected void Movement() {
		GearUpdate ();
		Friction ();
		UpdateVelocity ();
		Vector3 tVec = new Vector3 (this.transform.forward.x * velocity * earthingFactor, 
		                            rigidbody.velocity.y, 
		                            this.transform.forward.z * velocity * earthingFactor);
		rigidbody.velocity = tVec;
		UpdateRPM ();
	}
	protected void UpdateVelocity() {
		velocity += acceleration * Time.deltaTime;
		checkValid ();
	}
	protected void Friction() {
		acceleration -= accelerationFactor * Time.deltaTime;
		velocity -= accelerationFactor * 0.5f * Time.deltaTime;
		checkValid ();
	}
	protected void checkValid() {
		if (velocity > limitedVelocity) {
			velocity = limitedVelocity;
		} else if (velocity < 0) {
			velocity = 0.0f;
		}
		if (acceleration > limitedAcceleration) {
			acceleration = limitedAcceleration;
		} else if (acceleration < 0) {
			acceleration = 0;
		}
	}
	protected void UpdateRPM() {
		if (limitedVelocity == 0) {
			RPM = 0.0f;
			return;
		}
		RPM = velocity / (gearFactor * maximumVelocity);
		if (RPM < 0.1f) {
			RPM = 0.1f;
		}
	}

/* Initialize */
	public void InitializeCar() {
		InitializeBase ();
	}
	
/* Modify velocity when OnCollision */
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.name != "Terrain") {
			velocity = velocity * collisionFactor;
		}
	}
}
