using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MeterUI : MonoBehaviour {
	private Image ClutchHighlight;
	private Image KMHArrow;
	private Text KMHState;
	private Text GearState;
	
	private Image RPMArrow;
	private Text RPMState;
	private Text DurabilityState;

	private float KMH;
	private float RPM;
	private float durability;

	void Start () {
		ClutchHighlight = GameObject.Find ("ClutchHighlight").GetComponent<Image> ();
		KMHArrow = GameObject.Find ("KMHArrow").GetComponent<Image> ();
		KMHState = GameObject.Find ("KMHState").GetComponent<Text> ();
		GearState = GameObject.Find ("GearState").GetComponent<Text> ();
		RPMArrow = GameObject.Find ("RPMArrow").GetComponent<Image> ();
		RPMState = GameObject.Find ("RPMState").GetComponent<Text> ();
		DurabilityState = GameObject.Find ("DurabilityState").GetComponent<Text> ();
		KMH = 0.0f;
		RPM = 0.0f;
		durability = 100.0f;
	}

	void Update () {
		UpdateClutchHighlight ();
		UpdateKMHArrow ();
		UpdateKMHState ();
		UpdateGearState ();

		UpdateRPMArrow ();
		UpdateRPMState ();
		UpdateDurabilityState ();
	}

	void UpdateClutchHighlight() {
		if (Player.playerClutch) {
			ClutchHighlight.enabled = true;
		} else {
			ClutchHighlight.enabled = false;
		}
	}
	void UpdateKMHArrow() {
		Quaternion tempRotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, -KMH * 1.333333f));
		KMHArrow.transform.rotation = tempRotation;
	}
	void UpdateKMHState() {
		KMH = Player.playerVelocity * 4.0f;
		float tempKMH = Mathf.Round (KMH * 10.0f);
		string tempKMHS = (tempKMH / 10.0f).ToString ();
		if (tempKMH % 10 == 0) {
			tempKMHS = tempKMHS + ".0";
		}
		KMHState.text = tempKMHS;
	}
	void UpdateGearState() {
		if (Player.playerCarState == CarBase.CarState.ENGINE_OFF) {
			GearState.text = "";
		} else if (Player.playerCarState == CarBase.CarState.GEARS_1) {
			GearState.text = "1";
		} else if (Player.playerCarState == CarBase.CarState.GEARS_2) {
			GearState.text = "2";
		} else if (Player.playerCarState == CarBase.CarState.GEARS_3) {
			GearState.text = "3";
		} else if (Player.playerCarState == CarBase.CarState.GEARS_4) {
			GearState.text = "4";
		} else if (Player.playerCarState == CarBase.CarState.GEARS_5) {
			GearState.text = "5";
		} else if (Player.playerCarState == CarBase.CarState.GEARS_N) {
			GearState.text = "N";
		} else if (Player.playerCarState == CarBase.CarState.GEARS_R) {
			GearState.text = "R";
		} 
	}

	void UpdateRPMArrow() {
		Quaternion tempRotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, -RPM * 0.03f));
		RPMArrow.transform.rotation = tempRotation;
	}
	void UpdateRPMState() {
		RPM = Player.playerRPM * 7000.0f;
		float tempRPM = Mathf.Round (RPM);
		RPMState.text = tempRPM.ToString ();
	}
	void UpdateDurabilityState() {
		durability = Player.playerDurability * 100.0f;
		float tempDurability = Mathf.Round (durability);
		DurabilityState.text = tempDurability.ToString ();
	}
}
