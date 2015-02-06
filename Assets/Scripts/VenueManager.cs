using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VenueManager : MonoBehaviour {
	List<Venue> venues;
	public Venue venuePrefab;

	// Use this for initialization
	void Start () {
		if (!venuePrefab) {
			Debug.LogError("Unable to start Venue Manager: No venue prefab is set.");
		}

		venues = new List<Venue>();

		// @TODO Load these from an external source.

		CreateVenue("Civic Center", "Meh. A small venue with room for only a few people.", 1000.0f, 0.00f, 2981, 0.5f);
		CreateVenue("Sportatorium", "The southern equivalent to MSG", 50000.0f, 0.02f, 4500, 0.8f);
		CreateVenue("MSG", "Primetime venue in NYC.", 100000.0f, 0.05f, 18200, 0.7f);
	}
	
	public List<Venue> GetVenues() {
		return venues;
	}

	public Venue CreateVenue(string name, string description, float baseCost, float gatePercentage, int capacity, float popularity) {
		Venue venue = Instantiate(venuePrefab) as Venue;
		venue.transform.SetParent(transform, false);
		venue.Initialize(name, description, baseCost, gatePercentage, capacity, popularity);
		venues.Add (venue);
		return venue;
	}
}
