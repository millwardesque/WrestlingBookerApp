using UnityEngine;
using System.Collections;

public class WrestlingEvent : MonoBehaviour {
	public string eventName;
	public int ticketsSold;
	public float revenue;
	public float ticketPrice = 20.0f;

	EventType type;
	public EventType Type { get; set; }

	Venue eventVenue;
	public Venue EventVenue { get; set; }
}
