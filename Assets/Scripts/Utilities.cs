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


	public static float RangeFromPercentage(float percentage, Vector2 range) {
		return percentage * (range.y - range.x) + range.x;
	}

	public static int RandomRangeInt(Vector2 range) {
		return Random.Range ((int)range.x, (int)range.y);
	}
	
	public static float RandomRange(Vector2 range) {
		return Random.Range (range.x, range.y);
	}

	public static float Fuzzify(float value, float fuzziness) {
		return value + Random.Range(-fuzziness, fuzziness);
	}

	public static float RangePercentage(float value, Vector2 range) {
		if (float.Epsilon >= Mathf.Abs(range.y - range.x)) {
			return 1f;
		}
		else {
			return (value - range.x) / (range.y - range.x);
		}
	}
}
