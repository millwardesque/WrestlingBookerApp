using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

	public static string FractionString(float percentage, int scale) {
		return string.Format ("{0} / {1}", Mathf.RoundToInt(percentage * scale), scale);
	}
}
