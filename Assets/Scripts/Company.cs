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

	public void Initialize(string name, float money, int maxRosterSize, int phase, List<Wrestler> roster, bool isInAlliance) {
		this.id = GetInstanceID().ToString();
		this.companyName = name;
		this.money = money;
		this.maxRosterSize = maxRosterSize;
		this.phase = phase;
		this.roster = roster;
		this.isInAlliance = isInAlliance;
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

	void AttemptToUnlockVenue() {
		bool unlockNewVenue = (Random.Range(0, 5) == 0);
		if (unlockNewVenue) {
			Venue newVenue = GameManager.Instance.GetVenueManager().GetRandomAvailableVenue(this);
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
