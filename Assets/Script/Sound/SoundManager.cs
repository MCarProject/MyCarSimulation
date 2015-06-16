using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static AudioSource carStartingEffect;
	AudioSource carDrivingEffect;
	static AudioSource carHornEffect;

	// Use this for initialization
	void Start () {
		carStartingEffect = GameObject.Find ("CarStartingEffect").GetComponent<AudioSource> ();
		carDrivingEffect = GameObject.Find ("CarDrivingEffect").GetComponent<AudioSource> ();
		carHornEffect = GameObject.Find ("CarHornEffect").GetComponent<AudioSource> ();

		carStartingEffect.volume = GlobalConfig.worldEffect;
		carDrivingEffect.volume = GlobalConfig.worldEffect;
		carHornEffect.volume = GlobalConfig.worldEffect * 0.7f;

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

	public static void HornEffectManager(bool on) {
		if (on) {
			if (!carHornEffect.isPlaying) {
				carHornEffect.Play ();
			}
		} else {
			carHornEffect.Stop ();
		}
	}
}
