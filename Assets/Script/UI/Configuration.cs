using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Configuration : MonoBehaviour {
	public Slider backgound;
	public Slider effect;
	public Slider speed;
	public Slider accel;
	public Slider stability;

	public Text background_text;
	public Text effect_text;
	public Text speed_text;
	public Text accel_text;
	public Text stability_text;

	void Start () {
		//accel.OnDrag (SetGlobalConfig);
	}

	void Update () {
		UpdateText ();
	}

	void UpdateText() {
		float temp;
		temp = Mathf.Round (backgound.value * 100);
		background_text.text = temp.ToString() + "%";
		temp = Mathf.Round (effect.value * 100);
		effect_text.text = temp.ToString() + "%";
		speed_text.text = speed.value.ToString();
		accel_text.text = accel.value.ToString();
		temp = Mathf.Round (stability.value * 100) / 100;
		stability_text.text = temp.ToString();
	}

	public void SetGlobalConfig() {
		GlobalConfig.worldBackground = backgound.value;
		GlobalConfig.worldEffect = effect.value;
		GlobalConfig.playerSpeed = speed.value;
		GlobalConfig.playerAcceleration = accel.value;
		GlobalConfig.playerStability = stability.value;
	}
}
