using UnityEngine;
using System.Collections;

public class Company : MonoBehaviour {
	public string companyName;
	public float money;

	public bool Save(string keyPrefix) {
		PlayerPrefs.SetInt(keyPrefix, 1);
		PlayerPrefs.SetString (keyPrefix + ".name", companyName);
		PlayerPrefs.SetFloat (keyPrefix + ".money", money);

		return true;
	}

	public bool Load(string keyPrefix) {
		if (!IsSaved(keyPrefix)) {
			return false;
		}
	
		if (PlayerPrefs.HasKey(keyPrefix + ".money")) {
			money = PlayerPrefs.GetFloat(keyPrefix + ".money");
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".name")) {
			companyName = PlayerPrefs.GetString(keyPrefix + ".name");
		}

		return true;
	}

	public bool IsSaved(string keyPrefix) {
		return PlayerPrefs.HasKey(keyPrefix);
	}
}
