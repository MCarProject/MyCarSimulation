using UnityEngine;
using System.Collections;

public class CarBase : ObjectBase {
	public enum CarState { ENGINE_OFF, GEARS_N, GEARS_R, GEARS_1, GEARS_2, 
					GEARS_3, GEARS_4, GEARS_5, CLUTCHED, REPAIRED };

	private float RPM;					//엔진회전속도 (0~1)
	private float maximumVelocity;		//최대속도
	private float limitedVelocity;		//한계속도
	private float velocity;				//속도
	private float maximumAcceleration;	//최대가속도
	private float limitedAcceleration;	//한계가속도
	private float acceleration;			//가속도
	private float accelerationFactor;	//가속계수
	private float gearFactor;			//기어계수
	private float collisionFactor;		//충돌계수
	private float turnRate;				//회전율
	private float durability;			//내구도
	private CarState carState;			//기어상태

	private Rigidbody rigidbody;

	void Start () {
		RPM = 0.0f;
		maximumVelocity = 15.0f;
		limitedVelocity = 0.0f;
		velocity = 0.0f;
		maximumAcceleration = 1.5f;
		limitedAcceleration = 0.0f;
		acceleration = 0.0f;
		accelerationFactor = maximumAcceleration;
		gearFactor = 0.0f;
		collisionFactor = 0.2f;
		turnRate = 50.0f;
		durability = 1.0f;
		carState = CarState.GEARS_4;

		rigidbody = this.GetComponent<Rigidbody> ();
		SetGear ();
	}
	void Update() {
		UpdateVelocity ();
		Friction ();
		UpdateRPM ();
		Movement ();
		Debug.Log ("v:" + velocity + "/" + limitedVelocity + ", a:" + acceleration +
			"/" + limitedAcceleration + ", g:" + gearFactor + ", S:" + carState +
		           ", RPM:" + RPM);
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
		if (carState == CarState.CLUTCHED) {
			carState = newCarState;
		} else {
			OffEngine();
		}
		SetGear ();
	}
	void SetGear() {
		if (carState == CarState.ENGINE_OFF ||
			carState == CarState.GEARS_N ||
			carState == CarState.GEARS_R ||
			carState == CarState.REPAIRED ||
			carState == CarState.CLUTCHED) {
			gearFactor = 0.0f;
			limitedVelocity = 0.0f;
			limitedAcceleration = 0.0f;
			return;
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
		limitedVelocity = gearFactor * maximumVelocity;
		limitedAcceleration = (1 / gearFactor) * maximumAcceleration;
	}


/* Movement Processing */
	public void Accelerate() {
		//acceleration += (1 / gearFactor) * accelerationFactor * Time.deltaTime;
		acceleration = limitedAcceleration;
		checkValid ();
	}
	public void Decelerate() {
		//acceleration -= maximumAcceleration * Time.deltaTime;
		acceleration = 0;
		velocity -= accelerationFactor * 2.0f * Time.deltaTime;
		checkValid ();
	}
	void UpdateVelocity() {
		velocity += acceleration * Time.deltaTime;
		checkValid ();
	}
	void Friction() {
		acceleration -= accelerationFactor * Time.deltaTime;
		velocity -= accelerationFactor * 0.5f * Time.deltaTime;
		checkValid ();
	}
	public void Turn(float axis) {
		if (velocity == 0) {
			return;
		}
		this.transform.Rotate (0, (axis * turnRate * Time.deltaTime), 0);
	}
	void Movement() {
		Vector3 tVec = new Vector3 (this.transform.forward.x * velocity, 
		                            rigidbody.velocity.y, 
		                            this.transform.forward.z * velocity);
		rigidbody.velocity = tVec;
	}
	void checkValid() {
		if (velocity > limitedVelocity) {
			velocity = limitedVelocity;
		} else if (velocity < 0) {
			velocity = 0;
		}
		if (acceleration > limitedAcceleration) {
			acceleration = limitedAcceleration;
		} else if (acceleration < 0) {
			acceleration = 0;
		}
	}
	void UpdateRPM() {
		if (limitedVelocity == 0) {
			RPM = 0.0f;
			return;
		}
		RPM = velocity / limitedVelocity;
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
