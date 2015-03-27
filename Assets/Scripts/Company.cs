using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Company : MonoBehaviour {
	public string id;
	public string companyName;
	public float money;
	public int maxRosterSize;
	public int phase = -1;
	public List<HistoricalWrestlingEvent> eventHistory = new List<HistoricalWrestlingEvent>();
	public List<Venue> unlockedVenues = new List<Venue>();
	public List<WrestlingMatchType> unlockedMatchTypes = new List<WrestlingMatchType>();
	public bool isInAlliance;
	public List<Wrestler> roster = new List<Wrestler>();

	public void Awake() {
		this.id = GetInstanceID().ToString();
	}

	public void Initialize(string name, float money, int maxRosterSize, int phase, List<Wrestler> roster, bool isInAlliance) {
		this.id = GetInstanceID().ToString();
		this.companyName = name;
		this.name = name;
		this.money = money;
		this.maxRosterSize = maxRosterSize;
		this.phase = phase;
		this.roster = roster;
		this.isInAlliance = isInAlliance;
	}

	/// <summary>
	/// Wrestlers loaded by ES2 are game objects separate from wrestlers loaded by the wrestler manager, leading to duplicates.
	/// Call this function to replace the wrestler clones with the canonical ones in the wrestler manager.
	/// </summary>
	public void SyncRoster() {
		for (int i = 0; i < roster.Count; ++i) {
			Wrestler wrestler = roster[i];
			Wrestler canonicalWrestler = WrestlerManager.Instance.GetWrestler(wrestler.wrestlerName);
			if (canonicalWrestler != null) {
				Destroy (wrestler.gameObject);
				roster[i] = canonicalWrestler;
			}
		}
	}

	public void SyncVenues() {
		for (int i = 0; i < unlockedVenues.Count; ++i) {
			Venue venue = unlockedVenues[i];
			Venue canonicalVenue = VenueManager.Instance.GetVenue(venue.venueName);
			if (canonicalVenue != null) {
				Destroy (venue.gameObject);
				unlockedVenues[i] = canonicalVenue;
			}
		}
	}

	public bool CanAddWrestlers() {
		return maxRosterSize > roster.Count;
	}

	public void AddWrestlerToRoster(Wrestler wrestler) {
		if (CanAddWrestlers()) {
			roster.Add(wrestler);
			Save ();
		}
		else {
			throw new UnityException("Unable to add wrestler to company '" + companyName + ": The roster is full");
		}
	}

	public void RemoveFromRoster(Wrestler wrestler) {
		roster.Remove(wrestler);
		Save ();
	}

	public List<Wrestler> GetRoster() {
		return roster;
	}

	public void Save() {
		string companyLocation = CompanyManager.Instance.CompanyFilename + "?tag=" + id;
		ES2.Save(this, companyLocation);
	}

	public void AddEvent(WrestlingEvent wrestlingEvent) {
		float oldPopularity = this.Popularity;
		eventHistory.Insert(0, wrestlingEvent.AsHistoricalEvent());
		float newPopularity = this.Popularity;

		if (newPopularity >= oldPopularity) {
			AttemptToUnlockVenue();
		}
		Save ();
	}

	public void AddEvent(HistoricalWrestlingEvent wrestlingEvent) {
		float oldPopularity = this.Popularity;
		eventHistory.Insert(0, wrestlingEvent);
		float newPopularity = this.Popularity;
		
		if (newPopularity >= oldPopularity) {
			AttemptToUnlockVenue();
		}
		Save ();
	}

	public void UnlockVenue(Venue venue) {
		unlockedVenues.Add (venue);
		Save ();
	}

	void AttemptToUnlockVenue() {
		bool unlockNewVenue = (Random.Range(0, 5) == 0);
		if (unlockNewVenue) {
			Venue newVenue = VenueManager.Instance.GetRandomAvailableVenue(this);
			if (newVenue != null) {
				GameManager.Instance.GetGUIManager().AddNotification("Venue '" + newVenue.venueName + "' unlocked");
				unlockedVenues.Add(newVenue);
				Save ();
			}
		}
	}

	public void AttemptUnlockMatchTypeByVenue(Venue venue) {
		bool unlockNewMatchType = (Random.Range(0, 3) == 0);
		if (unlockNewMatchType) {
			WrestlingMatchType matchType = GameManager.Instance.GetMatchTypeManager().GetMatchType(venue.unlockableMatchType);
			if (matchType != null && matchType.phase <= phase && unlockedMatchTypes.Find(x => x.typeName == matchType.typeName) == null) {
				GameManager.Instance.GetGUIManager().AddNotification("Match type '" + matchType.typeName + "' unlocked");
				unlockedMatchTypes.Add (matchType);
				Save ();
			}
		}
	}

	public float Popularity {
		get {
			int maxHistoryLength = Mathf.Min (10, eventHistory.Count);	// Maximum number of events in the past to search
			float eventRatingSum = 0.0f;
			for (int i = 0; i < eventHistory.Count; ++i) {
				eventRatingSum += eventHistory[i].rating;
			}

			return (maxHistoryLength == 0 ? 0.1f : eventRatingSum / maxHistoryLength);
		}
	}
}
