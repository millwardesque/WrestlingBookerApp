using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

	public static string FractionString(float percentage, int scale) {
		return string.Format ("{0} / {1}", Mathf.RoundToInt(percentage * scale), scale);
	}

	public static string AlphaRating(float percentage) {
		if (percentage >= 0.8) {
			return "A";
		}
		else if (percentage >= 0.6) {
			return "B";
		}
		else if (percentage >= 0.4) {
			return "C";
		}
		else if (percentage >= 0.2) {
			return "D";
		}
		else {
			return "E";
		}
	}
}
