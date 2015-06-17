using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour {
	bool isOnMenu = false;

	public GameObject gameMenuSet;

	void Start () {
		gameMenuSet.SetActive (false);
	}

	void Update () {
		SetMenuUI ();
		UpdateCursor ();
	}

	void SetMenuUI() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			isOnMenu = !isOnMenu;
			
			gameMenuSet.SetActive (true);
			if (!isOnMenu) {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				gameMenuSet.SetActive (false);
			}
		}
	}
	void UpdateCursor() {
		if (isOnMenu) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	public void OnReturn() {
		isOnMenu = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		gameMenuSet.SetActive (false);
	}
	public void OnMenu() {
		Application.LoadLevel ("Main");
	}
	public void OnExit() {
		Application.Quit ();
	}
}
