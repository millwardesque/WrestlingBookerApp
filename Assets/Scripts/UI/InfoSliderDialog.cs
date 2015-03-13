using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class InfoSliderDialog : UIDialog {
	public Text message;
	public Slider slider;
	
	string afterMessage;
	float finalValue;
	float minValue;
	float duration;
	
	void Start() {
		if (message == null) {
			LogStartError("Message isn't set");
		}
	}
	
	public void Initialize(string title, string beforeMessage, string afterMessage, float minValue, float maxValue, float finalValue, float duration, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null, string okLabel = "OK", string cancelLabel = "Cancel") {
		this.message.text = beforeMessage;
		this.afterMessage = afterMessage;
		this.finalValue = finalValue;
		this.minValue = minValue;
		this.duration = duration;
		slider.minValue = minValue;
		slider.maxValue = maxValue;
		slider.value = 0;

		base.Initialize(title, okAction, canCancel, cancelAction, okLabel, cancelLabel);

		StartCoroutine(AnimateSlider());
	}

	IEnumerator AnimateSlider() {
		float unitsPerSecond = (finalValue - minValue) / duration;

		HideOKButton();
		while (slider.value < finalValue) {
			slider.value += Time.deltaTime * unitsPerSecond;
			yield return null;
		}
		if (afterMessage != "") {
			this.message.text = afterMessage;
		}
		yield return new WaitForSeconds(0.5f);

		ShowOKButton();
	}
}
