using UnityEngine;
using System.Collections;
using System.Collections.Generic; //allows us to use lists

public class AICar : MonoBehaviour {
	
	public Transform pathGroup;
	public float maxSteer = 150.0f;
	public WheelCollider WheelFL;
	public WheelCollider WheelFR;
	public int currentPathObj;
	float distFromPath = 10.0f;		//회전시작거리
	float distDeceller = 20.0f;		//감속시작거리
	float maxTorque = 150.0f;
	float currentSpeed;
	float topSpeed = 40.0f;
	float decellerationSpeed = 50.0f;

	bool onCorner = false;
	float cornerTimer;
	
	private List<Transform> path;
	public WheelCollider[] wheelCollider = new WheelCollider[4];
	public Transform[] wheelTransform = new Transform[4];

	void Start () {
		GetPath();
	}
	
	void GetPath () {
		Transform[] path_objs = pathGroup.GetComponentsInChildren<Transform>();
		path = new List<Transform>();
		for (int i = 0; i < path_objs.Length; i++) {
			if (path_objs[i] != pathGroup) {
				path.Add(path_objs[i]);
			}
		}
	}
	
	void Update () {
		Turn ();
		Move ();
		UpdateWheelTransform ();
	}
	
	void Turn () {
		Vector3 steerVector = transform.InverseTransformPoint(new Vector3(path[currentPathObj].position.x, transform.position.y, path[currentPathObj].position.z));
		float newSteer = maxSteer * (steerVector.x / (steerVector.magnitude / 3));
		for (int i = 0; i<2; i++) {
			wheelCollider[i].steerAngle = newSteer;
		}
		
		Debug.Log (currentSpeed + ", " + newSteer + ", " + steerVector);

		if (steerVector.magnitude <= distFromPath) {
			currentPathObj++;
		}
		if (steerVector.magnitude <= distDeceller) {
			onCorner = true;
			cornerTimer = 2.0f;
		}	
		if (cornerTimer < 0) {
			onCorner = false;
		}
		if (currentPathObj >= path.Count) {
			currentPathObj = 0;
		}
	}

	void Move() {
		currentSpeed = 2 * Mathf.PI * wheelCollider[0].radius * wheelCollider[0].rpm * 60 / 1000;
		currentSpeed = Mathf.Round (currentSpeed);
		if(currentSpeed < topSpeed) {
			for (int i = 0; i<2; i++) {
				wheelCollider[i].motorTorque = maxTorque;
				wheelCollider[i].brakeTorque = 0;
			}
		} else {
			for (int i = 0; i<2; i++) {
				wheelCollider[i].motorTorque = 0;
				wheelCollider[i].brakeTorque = maxTorque;
			}
		}
		if (onCorner) {
			for (int i = 0; i<2; i++) {
				wheelCollider[i].motorTorque = 0;
				wheelCollider[i].brakeTorque = decellerationSpeed;
				cornerTimer -= Time.deltaTime;
				Debug.Log (cornerTimer);
			}
		}
	}
	

	void UpdateWheelTransform () 
	{
		for (int i = 0; i<4; i++) {
			wheelTransform[i].Rotate (0, 0, wheelCollider[i].rpm / 60 * 360 * Time.deltaTime);
		
			RaycastHit hit;
			Vector3 wheelPos;
			if (Physics.Raycast (wheelCollider[i].transform.position, -wheelCollider[i].transform.up, out hit, wheelCollider[i].radius + wheelCollider[i].suspensionDistance)) {
				wheelPos = hit.point + wheelCollider[i].transform.up * wheelCollider[i].radius;
			} else {
				wheelPos = wheelCollider[i].transform.position - wheelCollider[i].transform.up * wheelCollider[i].suspensionDistance;
			}
			wheelTransform[i].position = wheelPos;
		}
		for (int i = 0; i<2; i++) {
			Vector3 temp = wheelTransform[i].eulerAngles;
			temp.y = this.transform.eulerAngles.y - wheelCollider[i].steerAngle;
			wheelTransform[i].eulerAngles = temp;
		}
	}
}