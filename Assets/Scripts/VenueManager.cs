using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class VenueManager : MonoBehaviour {
	List<Venue> venues = new List<Venue>();
	VenueGenerator venueGenerator = new VenueGenerator();

	public Venue venuePrefab;

	public static VenueManager Instance;

	public string VenueFilename {
		get { return SavedGameManager.Instance.CurrentGameID + ".venues"; }
	}

	// Use this for initialization
	void Awake () {
		if (null == Instance) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
			if (!venuePrefab) {
				Debug.LogError("Unable to start Venue Manager: No venue prefab is set.");
			}

			venueGenerator.Initialize();
		}
		else {
			Destroy (gameObject);
		}
	}

	public void LoadVenues() {
		if (!LoadSavedVenues()) {
			GenerateNewVenues();
		}
	}

	bool LoadSavedVenues() {
		if (ES2.Exists(VenueFilename)) {
			string[] tags = ES2.GetTags(VenueFilename);
			foreach (string tag in tags) {
				string venueLocation = VenueFilename + "?tag=" + tag;
				Venue venue = Instantiate(venuePrefab) as Venue;
				venue.transform.SetParent(transform, false);
				ES2.Load<Venue>(venueLocation, venue);
				venue.name = venue.venueName;
				venues.Add (venue);
			}
			return true;
		}
		else {
			return false;
		}
	}

	void GenerateNewVenues() {
		// Clean up existing data.
		foreach (Venue venue in venues) {
			Destroy(venue.gameObject);
		}
		venues.Clear();
		
		if (ES2.Exists(VenueFilename)) {
			ES2.Delete (VenueFilename);
		}

		// In this case, we load some venue data from a JSON file (like name / description / phase) and leave the generator to fill in the blanks.
		LoadFromJSON("venues");
		foreach (Venue venue in venues) {
			venueGenerator.GenerateVenue(venue, venue.phase);
			venue.Save();
		}
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
				int phase = venue["phase"].AsInt;
				string unlockableMatchType = venue["unlockableMatchType"];
				if (null == unlockableMatchType) {
					unlockableMatchType = "";
				}

				var matchTypePreferenceArray = venue["matchTypePreferences"].AsArray;
				Dictionary<string, float> matchTypePreferences = new Dictionary<string, float>();
				foreach (JSONNode type in matchTypePreferenceArray) {
					matchTypePreferences.Add(type["name"], type["preference"].AsFloat);
				}

				var matchFinishPreferenceArray = venue["matchFinishPreferences"].AsArray;
				Dictionary<string, float> matchFinishPreferences = new Dictionary<string, float>();
				foreach (JSONNode finish in matchFinishPreferenceArray) {
					matchFinishPreferences.Add(finish["name"], finish["preference"].AsFloat);
				}
				CreateVenue(name, description, baseCost, gatePercentage, capacity, popularity, matchTypePreferences, matchFinishPreferences, phase, unlockableMatchType);
			}
		}
		else {
			Debug.LogError("Unable to load event type data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}

	public void ClearSavedData() {
		GenerateNewVenues();
	}

	public Venue GetRandomAvailableVenue(Company company) {
		List<Venue> availableVenues = new List<Venue>();

		foreach (Venue venue in venues) {
			if (venue.phase <= company.phase && !company.unlockedVenues.Find( x => x.venueName == venue.venueName)) {
				availableVenues.Add (venue);
			}
		}

		if (availableVenues.Count > 0) {
			return availableVenues[ Random.Range (0, availableVenues.Count) ];
		}
		else {
			return null;
		}

	}

	public Venue GetVenue(string venueName) {
		return venues.Find( x => x.venueName == venueName );
	}
	
	public List<Venue> GetVenues() {
		return venues;
	}

	public void SaveData() {
		foreach (Venue venue in venues) {
			venue.Save();
		}
	}

	public Venue CreateVenue(string name, string description, float baseCost, float gatePercentage, int capacity, float popularity, Dictionary<string, float> matchTypePreferences, Dictionary<string, float> matchFinishPreferences, int phase, string unlockableMatchType) {
		Venue venue = Instantiate(venuePrefab) as Venue;
		venue.transform.SetParent(transform, false);
		venue.Initialize(name, description, baseCost, gatePercentage, capacity, popularity, matchTypePreferences, matchFinishPreferences, phase, unlockableMatchType);
		venues.Add (venue);

		return venue;
	}
}
