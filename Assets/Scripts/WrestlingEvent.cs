using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HistoricalWrestlingEvent {
	public string name;
	public int ticketsSold;
	public float revenue;
	public string type;
	public string venue;
	public float interest;
	public float rating;

	public void Initialize(string name, int ticketsSold, float revenue, string type, string venue, float interest, float rating) {
		this.name = name;
		this.ticketsSold = ticketsSold;
		this.revenue = revenue;
		this.type = type;
		this.venue = venue;
		this.interest = interest;
		this.rating = rating;
	}

	
	public void DeleteSaved(string keyPrefix) {
		PlayerPrefs.DeleteKey(keyPrefix);
		PlayerPrefs.DeleteKey(keyPrefix + ".name");
		PlayerPrefs.DeleteKey(keyPrefix + ".ticketsSold");
		PlayerPrefs.DeleteKey(keyPrefix + ".revenue");
		PlayerPrefs.DeleteKey(keyPrefix + ".type");
		PlayerPrefs.DeleteKey(keyPrefix + ".venue");
		PlayerPrefs.DeleteKey(keyPrefix + ".interest");
		PlayerPrefs.DeleteKey(keyPrefix + ".rating");
	}
	
	public bool Save(string keyPrefix) {
		PlayerPrefs.SetInt(keyPrefix, 1);
		PlayerPrefs.SetString (keyPrefix + ".name", name);
		PlayerPrefs.SetInt (keyPrefix + ".ticketsSold", ticketsSold);
		PlayerPrefs.SetFloat (keyPrefix + ".revenue", revenue);
		PlayerPrefs.SetString (keyPrefix + ".type", type);
		PlayerPrefs.SetString(keyPrefix + ".venue", venue);
		PlayerPrefs.SetFloat (keyPrefix + ".interest", interest);
		PlayerPrefs.SetFloat (keyPrefix + ".rating", rating);

		return true;
	}
	
	public bool Load(string keyPrefix) {
		if (!IsSaved(keyPrefix)) {
			return false;
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".name")) {
			name = PlayerPrefs.GetString(keyPrefix + ".name");
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".ticketsSold")) {
			ticketsSold = PlayerPrefs.GetInt(keyPrefix + ".ticketsSold");
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".revenue")) {
			revenue = PlayerPrefs.GetFloat(keyPrefix + ".revenue");
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".type")) {
			type = PlayerPrefs.GetString(keyPrefix + ".type");
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".venue")) {
			venue = PlayerPrefs.GetString(keyPrefix + ".venue");
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".interest")) {
			interest = PlayerPrefs.GetFloat(keyPrefix + ".interest");
		}

		if (PlayerPrefs.HasKey(keyPrefix + ".rating")) {
			rating = PlayerPrefs.GetFloat(keyPrefix + ".rating");
		}

		return true;
	}
	
	public bool IsSaved(string keyPrefix) {
		return PlayerPrefs.HasKey(keyPrefix);
	}
}

public class WrestlingEvent : MonoBehaviour {
	public string eventName;
	public int ticketsSold;
	public float revenue;
	public List<WrestlingMatch> matches = new List<WrestlingMatch>();
	public float ticketPrice = 20.0f;

	EventType type;
	public EventType Type { get; set; }

	Venue eventVenue;
	public Venue EventVenue { get; set; }

	public float EventInterest {
		get {	
			float interest = 0;

			// Calculate the average match interest and multiply it by the popularity of wrestling in the venue
			foreach (WrestlingMatch match in matches) {
				float matchInterest = 0;
				foreach (Wrestler wrestler in match.Participants) {
					matchInterest += (wrestler.popularity + wrestler.charisma / 2) / match.ParticipantCount;
				}
				interest += matchInterest / matches.Count;
			}
			interest *=  EventVenue.popularity;
			return Mathf.Clamp01(interest * Random.Range(0.7f, 1.3f));
		}
	}

	public float Rating {
		get {
			float rating = 0.0f;

			if (matches.Count > 0) {
				foreach (WrestlingMatch match in matches) {
					rating += match.rating;
				}
				rating /= matches.Count;
			}

			return rating;
		}
	}

	public HistoricalWrestlingEvent AsHistoricalEvent() {
		HistoricalWrestlingEvent historicalEvent = new HistoricalWrestlingEvent();
		historicalEvent.Initialize(eventName, ticketsSold, revenue, this.Type.typeName, this.EventVenue.name, this.EventInterest, this.Rating);
		return historicalEvent;
	}
}
