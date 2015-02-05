using UnityEngine;
using System.Collections;

public class Venue : MonoBehaviour {
	public string venueName;
	public string venueDescription;
	public float baseVenueCost;
	public float gatePercentage; // Scale of 0.0f - 1.0f
	public int capacity;
	public float popularity; // Scale of 0.0f - 1.0f

	public void Initialize(string venueName, string venueDescription, float baseVenueCost, float gatePercentage, int capacity, float popularity) {
		this.venueName = venueName;
		this.venueDescription = venueDescription;
		this.baseVenueCost = baseVenueCost;
		this.gatePercentage = Mathf.Clamp01(gatePercentage);
		this.capacity = capacity;
		this.popularity = Mathf.Clamp01(popularity);
	}

	public float GetVenueCost(WrestlingEvent wrestlingEvent) {
		return wrestlingEvent.ticketsSold * gatePercentage + baseVenueCost;
	}
}
