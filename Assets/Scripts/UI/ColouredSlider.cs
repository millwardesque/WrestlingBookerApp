using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Slider))]
public class ColouredSlider : MonoBehaviour {
	public Image sliderFill;
	public Color startColour;
	public Color midColour;
	public Color endColour;

	Slider slider;
	void Awake() {
		slider = GetComponent<Slider>();
		slider.onValueChanged.AddListener(UpdateSliderColour);
		UpdateSliderColour(slider.value);
	}

	void UpdateSliderColour(float value) {
		float normalized = (value - slider.minValue) / (slider.maxValue - slider.minValue);

		Color newColour;
		if (normalized < 0.5f) {
			normalized /= 0.5f;
			newColour = (1f - normalized) * startColour + normalized * midColour; 
		}
		else {
			normalized /= 0.5f;
			normalized -= 1f;
			newColour = (1f - normalized) * midColour + normalized * endColour; 
		}

		sliderFill.color = newColour;
	}
}
