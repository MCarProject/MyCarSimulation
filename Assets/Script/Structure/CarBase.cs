using UnityEngine;
using System.Collections;

public class CarBase : ObjectBase {
	public enum CarState { ENGINE_OFF, GEARS_N, GEARS_R, GEARS_1, GEARS_2, 
		GEARS_3, GEARS_4, GEARS_5 };
	private bool isClutched;				//클러치상태
	private bool isDownGear;				//기어조작상태
	private bool isBacking;					//후진상태

	protected float RPM;					//엔진회전속도	(0~1)
	protected float maximumVelocity;		//최대속도		(15~30 /s)
	protected float limitedVelocity;		//한계속도
	protected float velocity;				//속도
	protected float maximumReverseVelocity;	//역방향최대속도
	protected float reverseVelocty;			//역방향속도
	protected float standardAcceleration;	//기준가속도
	protected float acceleration;			//가속도
	protected float gearValue;				//기어수치
	protected float gearFactor;				//기어계수
	protected float collisionFactor;		//충돌계수	(0.2~0.6)
	protected float earthingFactor;			//접지계수	(0.5~0.9)
	protected float turnRate;				//회전율		(40~60 degree/s)
	protected float durability;				//내구도
	protected CarState carState;			//기어상태
	
	protected Rigidbody rigidBody;
	
	void Start () {
		isClutched = false;
		isDownGear = false;
		isBacking = false;
	}

	/* Input for engine control*/
	public void OnClutched(bool on) {
		if (on) {
			isClutched = true;
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
		acceleration = (1 / gearFactor) * standardAcceleration;
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
	
	/* Input for movement */
	public void Accelerate() {
		if (carState == CarState.ENGINE_OFF ||
		    carState == CarState.GEARS_N ||
		    carState == CarState.GEARS_R ||
		    isClutched) {
			return;
		}
		velocity += acceleration * Time.deltaTime;
		if (velocity > limitedVelocity) {
			velocity = limitedVelocity;
		}
		reverseVelocty -= standardAcceleration * 5.0f * Time.deltaTime;
		if (reverseVelocty < 0) {
			reverseVelocty = 0.0f;
		}
	}
	public void Reverse() {
		velocity -= standardAcceleration * 5.0f * Time.deltaTime;
		if (velocity < 0) {
			velocity = 0.0f;
		}
		if (carState != CarState.GEARS_R ||
		    isClutched) {
			return;
		}
		reverseVelocty += standardAcceleration * 2.5f * Time.deltaTime;
		if (reverseVelocty > maximumReverseVelocity) {
			reverseVelocty = maximumReverseVelocity;
		}
	}
	public void Brake() {
		velocity -= standardAcceleration * 5.0f * Time.deltaTime;
		if (velocity < 0) {
			velocity = 0.0f;
		}
		reverseVelocty -= standardAcceleration * 5.0f * Time.deltaTime;
		if (reverseVelocty > maximumReverseVelocity) {
			reverseVelocty = 0.0f;
		}
	}
	public void Turn(float axis) {
		if (velocity == 0 && reverseVelocty == 0) {
			return;
		}
		if (isBacking) {
			axis = -axis;
		}
		this.transform.Rotate (0, (axis * turnRate * Time.deltaTime), 0);
	}

	/* Movement processing */
	protected void Movement() {
		UpdateLimit ();
		Friction ();
		UpdateRPM ();
		float realVelocity;
		realVelocity = velocity - reverseVelocty;
		if (realVelocity < 0) {
			isBacking = true;
		} else {
			isBacking = false;
		}
		Vector3 realVector = new Vector3 (this.transform.forward.x * realVelocity * earthingFactor, 
		                            rigidBody.velocity.y, 
		                                  this.transform.forward.z * realVelocity * earthingFactor);
		rigidBody.velocity = realVector;
	}
	void UpdateLimit() {
		if (!isDownGear) {
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
	void Friction() {
		velocity -= standardAcceleration * 0.5f * Time.deltaTime;
		if (velocity < 0) {
			velocity = 0.0f;
		}
		reverseVelocty -= standardAcceleration * 0.5f * Time.deltaTime;
		if (reverseVelocty < 0) {
			reverseVelocty = 0.0f;
		}
	}
	void UpdateRPM() {
		if (limitedVelocity == 0) {
			RPM = 0.0f;
			return;
		}
		RPM = (velocity + reverseVelocty) / (gearFactor * maximumVelocity);
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
		if (collision.gameObject.name == "Cube") {
			velocity = velocity * 0.9f;
		} else if (collision.gameObject.name != "Terrain") {
			velocity = velocity * collisionFactor;
		}
	}
}
