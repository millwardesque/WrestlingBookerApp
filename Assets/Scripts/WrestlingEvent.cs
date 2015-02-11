using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
			float popularityVsMatchInterestSplit = 0.5f;
			float interest = EventVenue.popularity * popularityVsMatchInterestSplit;

			foreach (WrestlingMatch match in matches) {
				foreach (WrestlingTeam team in match.teams) {
					foreach (Wrestler wrestler in team.wrestlers) {
						interest += (1.0f - popularityVsMatchInterestSplit) * wrestler.popularity * wrestler.popularity / match.ParticipantCount;
					}
				}
			}
			return interest;
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
}
