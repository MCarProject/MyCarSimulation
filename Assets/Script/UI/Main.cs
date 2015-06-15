using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	public enum SceneState { MENU, SETTING, STAFF };
	public static SceneState state;

	public GameObject MainUISet;
	public GameObject SettingUISet;
	public GameObject StaffUISet;

	RaycastHit[] rayHit;

	void Start () {
		ChangeState (SceneState.MENU);
	}

	void Update () {
		GetInput ();
	}

	void GetInput() {
		Vector3 mouseP = Input.mousePosition;
		Vector3 worldP = Camera.main.ScreenToWorldPoint (mouseP);
		Ray ray = new Ray (worldP, Vector3.forward);

		Physics.RaycastAll (ray, 50.0f);

		Debug.DrawRay(worldP, Vector3.forward, Color.red, 0.5f);
	}

	void ChangeState (SceneState state) {
		MainUISet.SetActive (false);
		SettingUISet.SetActive (false);
		StaffUISet.SetActive (false);

		if (state == SceneState.MENU)
			MainUISet.SetActive (true);
		else if(state == SceneState.SETTING)
			SettingUISet.SetActive (true);
		else if(state == SceneState.STAFF)
			StaffUISet.SetActive (true);
	}
	
	public void OnNewGameButton() {
		Application.LoadLevel ("ModelTestScene");
	}
	public void OnSettingButton() {
		ChangeState (SceneState.SETTING);
	}
	public void OnStaffButton() {
		ChangeState (SceneState.STAFF);
	}
	public void OnExitButton() {
		Application.Quit ();
	}
	public void OnBackButton() {
		ChangeState (SceneState.MENU);
	}
}
