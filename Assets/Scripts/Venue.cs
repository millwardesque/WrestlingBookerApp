using UnityEngine;
using System.Collections;

public class Venue : MonoBehaviour {
	public string venueName;
	public string venueDescription;
	public float baseVenueCost;
	public float gatePercentage; // Scale of 0.0f - 1.0f

	public void Initialize(string venueName, string venueDescription, float baseVenueCost, float gatePercentage) {
		this.venueName = venueName;
		this.venueDescription = venueDescription;
		this.baseVenueCost = baseVenueCost;
		this.gatePercentage = gatePercentage;
	}

	public float GetVenueCost(WrestlingEvent wrestlingEvent) {
		return wrestlingEvent.ticketsSold * gatePercentage + baseVenueCost;
	}
}
