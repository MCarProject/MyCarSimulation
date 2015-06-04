using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static AudioSource carStartingEffect;
	AudioSource carDrivingEffect;

	// Use this for initialization
	void Start () {
		carStartingEffect = GameObject.Find ("CarStartingEffect").GetComponent<AudioSource> ();
		carDrivingEffect = GameObject.Find ("CarDrivingEffect").GetComponent<AudioSource> ();

		carStartingEffect.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		DrivingEffectManager ();
	}

	public static void StartingEffectManager(bool on) {
		if (on) {
			if(!carStartingEffect.isPlaying) {
				carStartingEffect.Play ();
			}
		} else {
			if(carStartingEffect.isPlaying) {
				carStartingEffect.Stop ();
			}
		}
	}
	
	void DrivingEffectManager() {
		if (Player.playerVelocity > 0.1f) {
			carDrivingEffect.volume = Mathf.Log(Player.playerVelocity / 2.0f);
			if(!carDrivingEffect.isPlaying) {
				carDrivingEffect.Play ();
			}
		} else {
			if(carDrivingEffect.isPlaying) {
				carDrivingEffect.Stop ();
			}
		}
	}
}
