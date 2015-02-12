using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Company : MonoBehaviour {
	public string companyName;
	public float money;
	public List<WrestlingEvent> eventHistory;
	public List<Wrestler> roster = new List<Wrestler>();
	GameManager gameManager;

	void Awake() {
		gameManager = GameObject.FindObjectOfType<GameManager>();
		if (gameManager == null) {
			Debug.LogError("Unable to start company: No object has the GameManager component.");
		}
	}
	
	public void DeleteSaved(string keyPrefix) {
		PlayerPrefs.DeleteKey(keyPrefix);
		PlayerPrefs.DeleteKey(keyPrefix + ".name");
		PlayerPrefs.DeleteKey(keyPrefix + ".money");
		PlayerPrefs.DeleteKey(keyPrefix + ".roster");
	}

	public bool Save(string keyPrefix) {
		PlayerPrefs.SetInt(keyPrefix, 1);
		PlayerPrefs.SetString (keyPrefix + ".name", companyName);
		PlayerPrefs.SetFloat (keyPrefix + ".money", money);

		string wrestlerNames = "";
		if (roster.Count > 0) {
			foreach (Wrestler wrestler in roster) {
				wrestlerNames += wrestler.wrestlerName + ",";
			}
			wrestlerNames = wrestlerNames.Substring(0, wrestlerNames.Length - 1); // Remove the trailing comma.
		}
		PlayerPrefs.SetString (keyPrefix + ".roster", wrestlerNames);

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

		if (PlayerPrefs.HasKey (keyPrefix + ".roster")) {
			string wrestlerNamesString = PlayerPrefs.GetString(keyPrefix + ".roster");

			if (wrestlerNamesString.Length > 0) {
				string[] wrestlerNames = wrestlerNamesString.Split(',');
				foreach (string wrestlerName in wrestlerNames) {
					roster.Add(gameManager.GetWrestlerManager().GetWrestler(wrestlerName));
				}
			}
		}

		return true;
	}

	public bool IsSaved(string keyPrefix) {
		return PlayerPrefs.HasKey(keyPrefix);
	}

	public void AddEvent(WrestlingEvent wrestlingEvent) {
		eventHistory.Add(wrestlingEvent);
	}

	public float GetEventTypeInterest(EventType type) {
		int eventCount = 0;
		float eventRatingSum = 0.0f;
		foreach (WrestlingEvent wrestlingEvent in eventHistory) {
			if (wrestlingEvent.Type.typeName == type.typeName) {
				eventCount++;
				eventRatingSum += wrestlingEvent.Rating;
			}
		}

		return (eventCount == 0 ? 0.1f : eventRatingSum / eventCount);
	}
}
