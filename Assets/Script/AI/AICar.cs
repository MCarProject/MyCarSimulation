using UnityEngine;
using System.Collections;
using System.Collections.Generic; //allows us to use lists

public class AICar : MonoBehaviour {
	public Rigidbody rigidbody;

	//Path finding
	public WorldManager worldManager;
	private List<Transform> path;
	public int currentPathObj;
	public float maxSteer = 50.0f;		//최대회전각
	float distFromPath = 10.0f;			//회전시작거리
	float distDeceller = 20.0f;			//감속시작거리
	float topSpeed = 60.0f;				//최대속력
	float decellerationSpeed = 80.0f;	//감속력
	float maxTorque = 200.0f;			//가속력
	float currentSpeed;
	bool onCorner = false;
	float cornerTimer;

	//Wheel Moving
	public WheelCollider[] wheelCollider = new WheelCollider[4];
	public Transform[] wheelTransform = new Transform[4];

	//Sensing
	float sensorLength = 6.0f;
	float frontSensorStartPoint = 2.5f;  
	float frontSensorSideDist = 1.0f;  
	float frontSensorsAngle = 30.0f;
	float sidewaySensorStartPoint = 1.1f;
	float sidewaySensorLength = 4.0f;  
	int sensorFlag = 0;
	float avoidSenstivity = 0.0f;

	//Respawning
	float respawnWait = 5f;  
	float respawnCounter = 0.0f; 

	void Start () {
		SetPath ();
		worldManager.GetRandomNode ();
	}

	void SetPath () {
		int myStartNode;
		int myEndNode;
		do {
			myStartNode = worldManager.GetMyNode (this.transform.position);
			myEndNode = worldManager.GetRandomNode ();
		} while(myStartNode == myEndNode);
		path = worldManager.NodeToTransform(worldManager.SearchPath (myStartNode, myEndNode));
	}
	
	void Update () {
		if (sensorFlag == 0) {
			Turn ();
		} else {
			AvoidTurn (avoidSenstivity);
		}
		Move ();
		UpdateWheelTransform ();
		Respawn ();
	}
	void FixedUpdate() {
		Sensors ();
	}
	
	void Turn () {
		Vector3 steerVector = transform.InverseTransformPoint(new Vector3(path[currentPathObj].position.x, transform.position.y, path[currentPathObj].position.z));
		float newSteer = maxSteer * (steerVector.x / (steerVector.magnitude / 3));
		for (int i = 0; i<2; i++) {
			wheelCollider[i].steerAngle = newSteer;
		}

		if (steerVector.magnitude <= distFromPath) {
			currentPathObj++;
		}
		if (steerVector.magnitude <= distDeceller) {
			onCorner = true;
			cornerTimer = 2.0f;
			topSpeed = 40.0f;
		}	
		if (cornerTimer < 0) {
			onCorner = false;
			topSpeed = 60.0f;
		}
		if (onCorner) {
			cornerTimer -= Time.deltaTime;
		}
		if (currentPathObj >= path.Count) {
			currentPathObj = 0;
			SetPath ();
		}
	}
	void AvoidTurn (float senstivity){
		for (int i=0; i<2; i++) {
			wheelCollider[i].steerAngle = maxSteer * senstivity;
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
	}

	void UpdateWheelTransform () 
	{
		for (int i = 0; i<4; i++) {
			wheelTransform[i].Rotate (0, 0, -wheelCollider[i].rpm / 60 * 360 * Time.deltaTime);
		
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
			temp.y = 270 + this.transform.eulerAngles.y + wheelCollider[i].steerAngle;
			wheelTransform[i].eulerAngles = temp;
		}
	}

	void Sensors(){
		Vector3 pos;
		RaycastHit hit;
		var rightAngle = Quaternion.AngleAxis (frontSensorsAngle, transform.up) * transform.forward;
		var leftAngle = Quaternion.AngleAxis (-frontSensorsAngle, transform.up) * transform.forward;
		
		sensorFlag = 0;
		avoidSenstivity = 0.0f;

		//Front Mid Sensor  
		pos = transform.position;
		pos.y += 0.5f;
		pos += transform.forward * frontSensorStartPoint;  
		Debug.DrawLine (pos, pos + transform.forward * sensorLength);
		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain") {  
				sensorFlag++;
				for (int i=0; i<2; i++) {
					wheelCollider [i].brakeTorque = decellerationSpeed;
				}
				Debug.DrawLine (pos, hit.point, Color.red);
			}  
		} else {  
			for (int i=0; i<2; i++) {
				wheelCollider [i].brakeTorque = 0.0f;
			}
		}
		//Front Right Sensor  
		pos += transform.right * frontSensorSideDist;
		Debug.DrawLine (pos, pos + transform.forward * sensorLength);
		Debug.DrawLine (pos, pos + rightAngle * sensorLength);
		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain") {  
				sensorFlag++;  
				avoidSenstivity -= 1.2f;
				Debug.DrawLine (pos, hit.point, Color.red);
			}  
		} else if (Physics.Raycast (pos, rightAngle, out hit, sensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				avoidSenstivity -= 0.8f;   
				sensorFlag++;  
				Debug.DrawLine (pos, hit.point, Color.red);
			}  
		}
		//Front left Sensor  
		pos -= 2 * transform.right * frontSensorSideDist;
		Debug.DrawLine (pos, pos + transform.forward * sensorLength);
		Debug.DrawLine (pos, pos + leftAngle * sensorLength);
		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				sensorFlag++;  
				avoidSenstivity += 1.2f;   
				Debug.DrawLine (pos, hit.point, Color.red);  
			}  
		} else if (Physics.Raycast (pos, leftAngle, out hit, sensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				sensorFlag++;  
				avoidSenstivity += 0.8f;  
				Debug.DrawLine (pos, hit.point, Color.red);  
			}  
		}
		pos = transform.position;
		pos.y += 0.5f;
		pos += transform.right * sidewaySensorStartPoint;
		Debug.DrawLine (pos, pos + transform.right * sidewaySensorLength);
		//Right SideWay Sensor  
		if (Physics.Raycast (pos, transform.right, out hit, sidewaySensorLength)) {
			if (hit.transform.tag != "Terrain") {  
				sensorFlag++;  
				avoidSenstivity -= 0.8f;  
				Debug.DrawLine (pos, hit.point, Color.red);  
			}  
		}
		pos -= 2 * transform.right * sidewaySensorStartPoint;
		Debug.DrawLine (pos, pos + -transform.right * sidewaySensorLength);
		//Left SideWay Sensor  
		if (Physics.Raycast (pos, -transform.right, out hit, sidewaySensorLength)) {
			if (hit.transform.tag != "Terrain") {  
				sensorFlag++;  
				avoidSenstivity += 0.8f;  
				Debug.DrawLine (pos, hit.point, Color.red);  
			}  
		}
		//Front Mid Sensor  
		pos = transform.position;
		pos.y += 0.5f;
		pos += transform.forward * frontSensorStartPoint;  
		Debug.DrawLine (pos, pos + transform.forward * sensorLength);
		if (avoidSenstivity == 0) {
			if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
				if (hit.transform.tag != "Terrain") {
					if (hit.normal.x < 0) {
						avoidSenstivity = 1.5f;
					} else {
						avoidSenstivity = -1.5f;
					}
					Debug.DrawLine (pos, hit.point, Color.red);
				}
			}
		}
	}

	void Respawn (){
		if (rigidbody.velocity.magnitude < 2) {  
			respawnCounter += Time.deltaTime;  
			if (respawnCounter >= respawnWait) {  
				if (currentPathObj == 0) {  
					transform.position = path [path.Count - 1].position;  
				} else {  
					transform.position = path [currentPathObj - 1].position;  
				}  
				respawnCounter = 0;
				Vector3 tempRotation = transform.eulerAngles;
				tempRotation.x = 0.0f;
				tempRotation.z = 0.0f;
				transform.eulerAngles = tempRotation;
			}  
		} 
	}
	
	void OnDrawGizmos() {
		if (path != null) {
			foreach (Transform n in path) {
				Gizmos.color = Color.white;
				Gizmos.DrawCube(n.position, new Vector3(0.5f,0.5f,0.5f));
			}
		}
	}
}