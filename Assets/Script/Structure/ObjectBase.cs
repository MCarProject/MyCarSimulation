using UnityEngine;
using System.Collections;

public class ObjectBase : MonoBehaviour {

	private int ID;
	private static int nextValidID = 0;

	private void SetID() {
		this.ID = nextValidID;
		nextValidID++;
	}
	public int GetID() {
		return this.ID;
	}

	public void InitializeBase() {
		SetID ();
	}
}
