using UnityEngine;
using System.Collections;

public class CarBase : ObjectBase {
	public enum CarState { ENGINE_OFF, GEARS_N, GEARS_R, GEARS_1, GEARS_2, 
		GEARS_3, GEARS_4, GEARS_5 };
	private bool clutched;
	private bool downGear;
	
	protected float RPM;					//엔진회전속도	(0~1)
	protected float maximumVelocity;		//최대속도		(15~30 /s)
	protected float limitedVelocity;		//한계속도
	protected float velocity;				//속도
	protected float standardAcceleration;	//기준가속도
	protected float acceleration;			//가속도
	protected float gearValue;				//기어수치
	protected float gearFactor;				//기어계수
	protected float collisionFactor;		//충돌계수	(0.2~0.6)
	protected float earthingFactor;			//접지계수	(0.5~0.9)
	protected float turnRate;				//회전율		(40~60 degree/s)
	protected float durability;				//내구도
	protected CarState carState;			//기어상태
	
	protected Rigidbody rigidbody;
	
	void Start () {
		clutched = false;
		downGear = false;
	}
	void Update() {
	}
	
	public void OnClutched(bool on) {
		if (on) {
			clutched = true;
		} else {
			clutched = false;
		}
	}
	
	//state true: downGear, false: upGear
	public void ChangeGear(bool state) {
		downGear = state;
		if (carState == CarState.ENGINE_OFF) {
			return;
		}
		if (!clutched) {
			carState = CarState.ENGINE_OFF;
			gearFactor = 0.0f;
		} else {
			if (!downGear) {
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
					gearFactor = 0.0f;
				}
			}
		}
		acceleration = (1 / gearFactor) * standardAcceleration;
	}
	
	//In Update()
	void UpdateLimit() {
		if (!downGear) {
			if (limitedVelocity >= velocity) {
				limitedVelocity = gearFactor * maximumVelocity;
			}
		} else {
			limitedVelocity -= 1.0f * Time.deltaTime;
			if(limitedVelocity < gearFactor * maximumVelocity) {
				limitedVelocity = gearFactor * maximumVelocity;
			}
		}
	}
	public void EngineControl() {
		if (carState == CarState.ENGINE_OFF) {
			carState = CarState.GEARS_N;
			gearFactor = 0.0f;
		} else {
			carState = CarState.ENGINE_OFF;
			gearFactor = 0.0f;
		}
	}
	
	/* Gear Processing */
	
	/* Movement Processing */
	public void Accelerate() {
		if (carState == CarState.ENGINE_OFF ||
		    carState == CarState.GEARS_N ||
		    clutched) {
			return;
		}
		velocity += acceleration * Time.deltaTime;
		checkValid ();
	}
	public void Decelerate() {
		velocity -= standardAcceleration * 5.0f * Time.deltaTime;
		checkValid ();
	}
	public void Turn(float axis) {
		if (velocity == 0) {
			return;
		}
		this.transform.Rotate (0, (axis * turnRate * Time.deltaTime), 0);
	}
	protected void Movement() {
		UpdateLimit ();
		Friction ();
		//UpdateVelocity ();
		Vector3 tVec = new Vector3 (this.transform.forward.x * velocity * earthingFactor, 
		                            rigidbody.velocity.y, 
		                            this.transform.forward.z * velocity * earthingFactor);
		rigidbody.velocity = tVec;
		UpdateRPM ();
	}
	protected void Friction() {
		velocity -= standardAcceleration * 0.5f * Time.deltaTime;
		checkValid ();
	}
	protected void checkValid() {
		if (velocity > limitedVelocity) {
			velocity = limitedVelocity;
		} else if (velocity < 0) {
			velocity = 0.0f;
		}
	}
	void UpdateRPM() {
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
