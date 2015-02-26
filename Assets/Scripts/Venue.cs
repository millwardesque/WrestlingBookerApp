using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Venue : MonoBehaviour {
	public string venueName;
	public string venueDescription;
	public float baseCost;
	public float gatePercentage; // Scale of 0.0f - 1.0f
	public int capacity;
	public float popularity; // Scale of 0.0f - 1.0f
	public int phase;
	public string unlockableMatchType;

	// These are look-up tables describing how much fans in this region like match types, finishes, etc.
	public Dictionary<string, float> matchTypePreferences = new Dictionary<string, float>();
	public Dictionary<string, float> matchFinishPreferences = new Dictionary<string, float>();

	List<string> seenMatchTypes = new List<string>();
	List<string> seenMatchFinishes = new List<string>();

	string StoragePrefix {
		get { return "venueChanges-" + venueName; }
	}

	public void Initialize(string venueName, string venueDescription, float baseCost, float gatePercentage, int capacity, float popularity, Dictionary<string, float> matchTypePreferences, Dictionary<string, float> matchFinishPreferences, int phase, string unlockableMatchType) {
		this.venueName = venueName;
		this.venueDescription = venueDescription;
		this.baseCost = baseCost;
		this.gatePercentage = Mathf.Clamp01(gatePercentage);
		this.capacity = capacity;
		this.popularity = Mathf.Clamp01(popularity);
		this.matchTypePreferences = matchTypePreferences;
		this.matchFinishPreferences = matchFinishPreferences;
		this.phase = phase;
		this.unlockableMatchType = unlockableMatchType;
	}

	public float GetVenueCost(WrestlingEvent wrestlingEvent) {
		return wrestlingEvent.TicketsSold * gatePercentage + baseCost;
	}

	public void AddSeenMatchType(WrestlingMatchType type) {
		if (!HasSeenMatchType(type)) {
			seenMatchTypes.Add(type.typeName);
			SaveAugmentedData();
		}
	}

	public bool HasSeenMatchType(WrestlingMatchType type) {
		return (null != seenMatchTypes.Find ( x => x == type.typeName));
	}

	public void AddSeenMatchFinish(WrestlingMatchFinish finish) {
		if (!HasSeenMatchFinish(finish)) {
			seenMatchFinishes.Add(finish.finishName);
		}
	}
	
	public bool HasSeenMatchFinish(WrestlingMatchFinish finish) {
		return (null != seenMatchFinishes.Find ( x => x == finish.finishName));
	}

	public float GetMatchTypePreference(WrestlingMatchType type) {
		if (matchTypePreferences.ContainsKey(type.typeName)) {
			return matchTypePreferences[type.typeName];
		}
		else {
			return 0.5f;
		}
	}

	public float GetMatchFinishPreference(WrestlingMatchFinish finish) {
		if (matchFinishPreferences.ContainsKey(finish.finishName)) {
			return matchFinishPreferences[finish.finishName];
		}
		else {
			return 0.5f;
		}
	}

	public void LoadAugmentedData() {
		string prefix = StoragePrefix;
		if (PlayerPrefs.HasKey(StoragePrefix + ".seenMatchTypes")) {
			seenMatchTypes = new List<string>(PlayerPrefs.GetString(prefix + ".seenMatchTypes").Split(';'));
		}

		if (PlayerPrefs.HasKey(StoragePrefix + ".seenMatchFinishes")) {
			seenMatchFinishes = new List<string>(PlayerPrefs.GetString(prefix + ".seenMatchFinishes").Split(';'));
		}
	}
	
	public void SaveAugmentedData() {
		string prefix = StoragePrefix;

		if (seenMatchTypes.Count > 0) {
			string seenMatchTypeNames = "";
			foreach (string matchType in seenMatchTypes) {
				seenMatchTypeNames += matchType + ";";
			}
			seenMatchTypeNames = seenMatchTypeNames.Substring(0, seenMatchTypeNames.Length - 1); // Remove the trailing separator
			PlayerPrefs.SetString(prefix + ".seenMatchTypes", seenMatchTypeNames);
		}


		if (seenMatchFinishes.Count > 0) {
			string seenMatchFinishNames = "";
			foreach (string matchFinish in seenMatchFinishes) {
				seenMatchFinishNames += matchFinish + ";";
			}
			seenMatchFinishNames = seenMatchFinishNames.Substring(0, seenMatchFinishNames.Length - 1); // Remove the trailing separator
			PlayerPrefs.SetString(prefix + ".seenMatchFinishes", seenMatchFinishNames);
		}
	}
	
	public void DeleteAugmentedData() {
		string prefix = StoragePrefix;
		PlayerPrefs.DeleteKey(prefix + ".seenMatchTypes");
		seenMatchTypes = new List<string>();

		PlayerPrefs.DeleteKey(prefix + ".seenMatchFinishes");
		seenMatchFinishes = new List<string>();
	}
}
