using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VenueManager : MonoBehaviour {
	List<Venue> venues;
	public Venue venuePrefab;

	// Use this for initialization
	void Start () {
		if (!venuePrefab) {
			Debug.LogError("Unable to start Venue Manager: No vendor prefab is set.");
		}

		venues = new List<Venue>();

		// @TODO Load these from an external source.
		Venue venue;

		venue = Instantiate(venuePrefab) as Venue;
		venue.Initialize("Civic Center", "Meh. A small venue with room for only a few people.", 1000.0f, 0.00f, 2981, 0.5f);
		venues.Add (venue);

		venue = Instantiate(venuePrefab) as Venue;
		venue.Initialize("Sportatorium", "The southern equivalent to MSG", 50000.0f, 0.02f, 4500, 0.8f);
		venues.Add (venue);

		venue = Instantiate(venuePrefab) as Venue;
		venue.Initialize("MSG", "Primetime venue in NYC.", 100000.0f, 0.05f, 18200, 0.7f);
		venues.Add (venue);
	}
	
	public List<Venue> GetVenues() {
		return venues;
	}
}
