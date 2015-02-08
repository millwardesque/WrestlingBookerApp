using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class VenueManager : MonoBehaviour {
	List<Venue> venues = new List<Venue>();
	public Venue venuePrefab;

	// Use this for initialization
	void Start () {
		if (!venuePrefab) {
			Debug.LogError("Unable to start Venue Manager: No venue prefab is set.");
		}

		LoadFromJSON("venues");
	}

	/// <summary>
	///  Loads the venues from a JSON file.
	/// </summary>
	/// <param name="filename">Filename.</param>
	void LoadFromJSON(string filename) {
		TextAsset jsonAsset = Resources.Load<TextAsset>(filename);
		if (jsonAsset != null) {
			string fileContents = jsonAsset.text;
			var N = JSON.Parse(fileContents);
			var venueArray = N["venues"].AsArray;
			foreach (JSONNode venue in venueArray) {
				string name = venue["name"];
				string description = venue["description"];
				float baseCost = venue["baseCost"].AsFloat;
				float gatePercentage = venue["gatePercentage"].AsFloat;
				int capacity = venue["capacity"].AsInt;
				float popularity = venue["popularity"].AsFloat;
				CreateVenue(name, description, baseCost, gatePercentage, capacity, popularity);
			}
		}
		else {
			Debug.LogError("Unable to load event type data from JSON at '" + filename + "': There was an error opening the file.");
		}
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
