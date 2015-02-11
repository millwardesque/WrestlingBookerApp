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

	// These are look-up tables describing how much fans in this region like match types, finishes, etc.
	public Dictionary<string, float> matchTypePreferences = new Dictionary<string, float>();
	public Dictionary<string, float> matchFinishPreferences = new Dictionary<string, float>();

	public void Initialize(string venueName, string venueDescription, float baseCost, float gatePercentage, int capacity, float popularity, Dictionary<string, float> matchTypePreferences, Dictionary<string, float> matchFinishPreferences) {
		this.venueName = venueName;
		this.venueDescription = venueDescription;
		this.baseCost = baseCost;
		this.gatePercentage = Mathf.Clamp01(gatePercentage);
		this.capacity = capacity;
		this.popularity = Mathf.Clamp01(popularity);
		this.matchTypePreferences = matchTypePreferences;
		this.matchFinishPreferences = matchFinishPreferences;
	}

	public float GetVenueCost(WrestlingEvent wrestlingEvent) {
		return wrestlingEvent.ticketsSold * gatePercentage + baseCost;
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
}
