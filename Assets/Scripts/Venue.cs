using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Venue : MonoBehaviour {
	public string id;
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

	public List<string> seenMatchTypes = new List<string>();
	public List<string> seenMatchFinishes = new List<string>();

	public void Initialize(string venueName, string venueDescription, float baseCost, float gatePercentage, int capacity, float popularity, Dictionary<string, float> matchTypePreferences, Dictionary<string, float> matchFinishPreferences, int phase, string unlockableMatchType) {
		this.id = GetInstanceID().ToString();
		this.venueName = venueName;
		this.name = venueName;
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
			Save();
		}
	}

	public bool HasSeenMatchType(WrestlingMatchType type) {
		return (null != seenMatchTypes.Find ( x => x == type.typeName));
	}

	public void AddSeenMatchFinish(WrestlingMatchFinish finish) {
		if (!HasSeenMatchFinish(finish)) {
			seenMatchFinishes.Add(finish.finishName);
			Save();
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

	public void Save() {
		string venueLocation = VenueManager.Instance.VenueFilename + "?tag=" + id;
		ES2.Save(this, venueLocation);
	}
}
