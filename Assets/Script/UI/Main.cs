using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();

	public enum SceneState { MENU, SETTING, STAFF };
	SceneState state;

	public SpriteRenderer panel;

	public SpriteRenderer btn_newgame;
	public SpriteRenderer btn_checkednewgame;
	public SpriteRenderer btn_setting;
	public SpriteRenderer btn_checkedsetting;
	public SpriteRenderer btn_staff;
	public SpriteRenderer btn_checkedstaff;
	public SpriteRenderer btn_exit;
	public SpriteRenderer btn_checkedexit;

	public SpriteRenderer volume_background;
	public SpriteRenderer volume_effect;
	public SpriteRenderer btn_on;
	public SpriteRenderer btn_checkedOn;
	public SpriteRenderer btn_off;
	public SpriteRenderer btn_checkedOff;
	public SpriteRenderer btn_on_;
	public SpriteRenderer btn_checkedOn_;
	public SpriteRenderer btn_off_;
	public SpriteRenderer btn_checkedOff_;
	public SpriteRenderer btn_back;
	public SpriteRenderer btn_checkedback;

	public SpriteRenderer staff;
	public SpriteRenderer btn_back_;
	public SpriteRenderer btn_checkedback_;

	SpriteRenderer[] btnSet;
	SpriteRenderer[] mainBtnSet;
	SpriteRenderer[] settingBtnSet;
	SpriteRenderer[] staffBtnSet;

	// Use this for initialization
	void Awake() {
	}

	void Start () {
		mainBtnSet = new SpriteRenderer[4];
		settingBtnSet = new SpriteRenderer[7];
     	staffBtnSet = new SpriteRenderer[3];

		state = SceneState.MENU;
		btnSet = FindObjectsOfType<SpriteRenderer> ();
		mainBtnSet[0] = btn_newgame;
		mainBtnSet[1] = btn_setting;
		mainBtnSet[2] = btn_staff;
		mainBtnSet[3] = btn_exit;
		
		settingBtnSet [0] = volume_background;
		settingBtnSet [1] = volume_effect;
		settingBtnSet [2] = btn_on;
		settingBtnSet [3] = btn_off;
		settingBtnSet [4] = btn_on_;
		settingBtnSet [5] = btn_off_;
		settingBtnSet [6] = btn_back;
		
		staffBtnSet [0] = staff;
		staffBtnSet [1] = btn_back_;
		staffBtnSet [2] = btn_checkedback_;
		OnMenu ();
	}
	
	// Update is called once per frame
	void Update () {
		GetInput ();
	}

	void GetInput() {
		Vector3 mouseP = Input.mousePosition;
		Vector3 worldP = Camera.main.ScreenToWorldPoint (mouseP);
		Debug.Log(worldP);

		Ray ray = new Ray (mouseP, Vector3.back);
		Debug.DrawRay(mouseP, Vector3.back, Color.red, 50.0f);
		//Debug.Log (mouseP);
	}

	void OnMenu() {
		state = SceneState.MENU;
		BtnReset ();
		foreach (SpriteRenderer btn in mainBtnSet) {
			btn.enabled = true;

		}
	}
	void OnSetting() {
		state = SceneState.SETTING;
		BtnReset ();
		foreach (SpriteRenderer btn in settingBtnSet) {
			btn.enabled = true;
		}
	}
	void OnStaff() {
		state = SceneState.STAFF;
		BtnReset ();
		foreach (SpriteRenderer btn in staffBtnSet) {
			btn.enabled = true;
		}
	}
	void BtnReset() {
		foreach (SpriteRenderer btn in btnSet) {
			btn.enabled = false;
		}
		panel.enabled = true;
	}
}
