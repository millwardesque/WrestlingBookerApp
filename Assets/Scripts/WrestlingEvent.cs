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
}
