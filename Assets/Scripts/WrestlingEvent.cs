using UnityEngine;
using System.Collections;

public class WrestlingEvent : MonoBehaviour {
	public string eventName;
	public int ticketsSold;
	public float revenue;
	public float ticketPrice;

	EventType type;
	public EventType Type {
		get { return type; }
		set {
			type = value;
		}
	}

	Venue eventVenue;
	public Venue EventVenue {
		get { return eventVenue; }
		set {
			eventVenue = value;
			ticketPrice = 100.0f;
		}
	}
}
