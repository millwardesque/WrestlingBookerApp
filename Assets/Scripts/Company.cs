﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Company : MonoBehaviour {
	public string companyName;
	public float money;
	public int maxRosterSize;
	public int phase = -1;
	public List<HistoricalWrestlingEvent> eventHistory = new List<HistoricalWrestlingEvent>();
	public List<Venue> unlockedVenues = new List<Venue>();
	public List<WrestlingMatchType> unlockedMatchTypes = new List<WrestlingMatchType>();
	public bool isInAlliance;

	List<Wrestler> roster = new List<Wrestler>();
	GameManager gameManager;

	void Awake() {
		gameManager = GameObject.FindObjectOfType<GameManager>();
		if (gameManager == null) {
			Debug.LogError("Unable to start company: No object has the GameManager component.");
		}
	}

	public void Initialize(string name, float money, int maxRosterSize, int phase, List<Wrestler> roster, bool isInAlliance) {
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
		}
		else {
			throw new UnityException("Unable to add wrestler to company '" + companyName + ": The roster is full");
		}
	}

	public void RemoveFromRoster(Wrestler wrestler) {
		roster.Remove(wrestler);
	}

	public List<Wrestler> GetRoster() {
		return roster;
	}
	
	public void DeleteSaved(string keyPrefix) {
		PlayerPrefs.DeleteKey(keyPrefix);
		PlayerPrefs.DeleteKey(keyPrefix + ".name");
		PlayerPrefs.DeleteKey(keyPrefix + ".money");
		PlayerPrefs.DeleteKey(keyPrefix + ".roster");
		PlayerPrefs.DeleteKey(keyPrefix + ".maxRosterSize");
		PlayerPrefs.DeleteKey(keyPrefix + ".phase");
		PlayerPrefs.DeleteKey(keyPrefix + ".isInAlliance");
		PlayerPrefs.DeleteKey(keyPrefix + ".unlockedVenues");
		PlayerPrefs.DeleteKey(keyPrefix + ".unlockedMatchTypes");

		ES2.Delete(keyPrefix + ".dat");
	}

	public bool Save(string keyPrefix) {
		PlayerPrefs.SetInt(keyPrefix, 1);
		PlayerPrefs.SetString (keyPrefix + ".name", companyName);
		PlayerPrefs.SetFloat (keyPrefix + ".money", money);
		PlayerPrefs.SetInt(keyPrefix + ".maxRosterSize", maxRosterSize);
		PlayerPrefs.SetInt (keyPrefix + ".phase", phase);
		PlayerPrefs.SetInt(keyPrefix + ".isInAlliance", (isInAlliance ? 1 : 0));

		// Save the past events.
		ES2.Save(eventHistory, keyPrefix + ".dat?tag=historicalEvents");

		// Save the roster
		string wrestlerNames = "";
		if (roster.Count > 0) {
			foreach (Wrestler wrestler in roster) {
				wrestlerNames += wrestler.wrestlerName + ",";
			}
			wrestlerNames = wrestlerNames.Substring(0, wrestlerNames.Length - 1); // Remove the trailing comma.
		}
		PlayerPrefs.SetString (keyPrefix + ".roster", wrestlerNames);

		// Save the unlocked venues.
		string unlockedVenueNames = "";
		if (unlockedVenues.Count > 0) {
			foreach (Venue venue in unlockedVenues) {
				unlockedVenueNames += venue.venueName + ",";
			}
			unlockedVenueNames = unlockedVenueNames.Substring(0, unlockedVenueNames.Length - 1); // Remove the trailing comma.
		}
		PlayerPrefs.SetString (keyPrefix + ".unlockedVenues", unlockedVenueNames);

		// Save the unlocked match types.
		string unlockedMatchTypeNames = "";
		if (unlockedMatchTypes.Count > 0) {
			foreach (WrestlingMatchType matchType in unlockedMatchTypes) {
				unlockedMatchTypeNames += matchType.typeName + ",";
			}
			unlockedMatchTypeNames = unlockedMatchTypeNames.Substring(0, unlockedMatchTypeNames.Length - 1); // Remove the trailing comma.
		}
		PlayerPrefs.SetString (keyPrefix + ".unlockedMatchTypes", unlockedMatchTypeNames);

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
			name = companyName;
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".maxRosterSize")) {
			maxRosterSize = PlayerPrefs.GetInt(keyPrefix + ".maxRosterSize");
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".phase")) {
			phase = PlayerPrefs.GetInt(keyPrefix + ".phase");
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".isInAlliance")) {
			isInAlliance = (PlayerPrefs.GetInt(keyPrefix + ".isInAlliance") == 1 ? true : false);
		}

		// Load the past events.
		if (ES2.Exists(keyPrefix + ".dat?tag=historicalEvents")) {
			eventHistory = ES2.LoadList<HistoricalWrestlingEvent>(keyPrefix + ".dat?tag=historicalEvents");
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

		if (PlayerPrefs.HasKey (keyPrefix + ".unlockedVenues")) {
			string unlockedVenueString = PlayerPrefs.GetString(keyPrefix + ".unlockedVenues");
			
			if (unlockedVenueString.Length > 0) {
				string[] venueNames = unlockedVenueString.Split(',');
				foreach (string venueName in venueNames) {
					Venue venue = gameManager.GetVenueManager().GetVenue(venueName);
					if (venue != null) {
						unlockedVenues.Add(venue);
					}
				}
			}
		}

		if (PlayerPrefs.HasKey (keyPrefix + ".unlockedMatchTypes")) {
			string unlockedMatchTypeString = PlayerPrefs.GetString(keyPrefix + ".unlockedMatchTypes");
			
			if (unlockedMatchTypeString.Length > 0) {
				string[] matchTypeNames = unlockedMatchTypeString.Split(',');
				foreach (string matchTypeName in matchTypeNames) {
					WrestlingMatchType matchType = gameManager.GetMatchTypeManager().GetMatchType(matchTypeName);
					if (matchType != null) {
						unlockedMatchTypes.Add(matchType);
					}
				}
			}
		}

		return true;
	}

	public bool IsSaved(string keyPrefix) {
		return PlayerPrefs.HasKey(keyPrefix);
	}

	public void AddEvent(WrestlingEvent wrestlingEvent) {
		float oldPopularity = this.Popularity;
		eventHistory.Insert(0, wrestlingEvent.AsHistoricalEvent());
		float newPopularity = this.Popularity;

		if (newPopularity >= oldPopularity) {
			AttemptToUnlockVenue();
		}
	}

	void AttemptToUnlockVenue() {
		bool unlockNewVenue = (Random.Range(0, 5) == 0);
		if (unlockNewVenue) {
			Venue newVenue = gameManager.GetVenueManager().GetRandomAvailableVenue(this);
			if (newVenue != null) {
				gameManager.GetGUIManager().AddNotification("Venue '" + newVenue.venueName + "' unlocked");
				unlockedVenues.Add(newVenue);
			}
		}
	}

	public void AttemptUnlockMatchTypeByVenue(Venue venue) {
		bool unlockNewMatchType = (Random.Range(0, 3) == 0);
		if (unlockNewMatchType) {
			WrestlingMatchType matchType = gameManager.GetMatchTypeManager().GetMatchType(venue.unlockableMatchType);
			if (matchType != null && matchType.phase <= phase && unlockedMatchTypes.Find(x => x.typeName == matchType.typeName) == null) {
				gameManager.GetGUIManager().AddNotification("Match type '" + matchType.typeName + "' unlocked");
				unlockedMatchTypes.Add (matchType);
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
