using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VenueGenerator {
	List<Vector2> baseCostRange = new List<Vector2>();
	List<Vector2> gatePercentageRange = new List<Vector2>();
	List<Vector2> capacityRange = new List<Vector2>();
	
	public void Initialize() {
		baseCostRange.Clear();
		baseCostRange.Add (new Vector2(500, 500));
		baseCostRange.Add (new Vector2(2500, 10000));
		baseCostRange.Add (new Vector2(50000, 100000));
		baseCostRange.Add (new Vector2(200000, 500000));
		
		gatePercentageRange.Clear();
		gatePercentageRange.Add (new Vector2(0f, 0.2f));
		gatePercentageRange.Add (new Vector2(0f, 0.2f));
		gatePercentageRange.Add (new Vector2(0f, 0.2f));
		gatePercentageRange.Add (new Vector2(0f, 0.2f));

		capacityRange.Clear();
		capacityRange.Add (new Vector2(500, 1000));
		capacityRange.Add (new Vector2(1500, 5000));
		capacityRange.Add (new Vector2(10000, 15000));
		capacityRange.Add (new Vector2(40000, 80000));
	}
	
	public void GenerateVenue(Venue venue, int phase) {
		string name = venue.venueName;
		string description = venue.venueDescription;
		int capacity = Utilities.RandomRangeInt(capacityRange[phase]);
		float capacityPercentage = Utilities.Fuzzify(Utilities.RangePercentage(capacity, capacityRange[phase]), 0.2f);

		float baseCost = Utilities.RangeFromPercentage(capacityPercentage, baseCostRange[phase]);
		float gatePercentage = Utilities.RandomRange(gatePercentageRange[phase]);

		float popularity = Random.Range (0f, 1f);

		string mostPopularMatchTypeName = null;
		float mostPopularMatchTypeAffinity = 0f;
		Dictionary<string, float> matchTypeAffinities = new Dictionary<string, float>();
		foreach (WrestlingMatchType type in WrestlingMatchTypeManager.Instance.GetMatchTypes()) {
			float affinity = Random.Range (0.1f, 1f);
			matchTypeAffinities.Add(type.typeName, affinity);

			if (affinity > mostPopularMatchTypeAffinity || mostPopularMatchTypeName == null) {
				mostPopularMatchTypeName = type.typeName;
				mostPopularMatchTypeAffinity = affinity;
			}
		}

		Dictionary<string, float> matchFinishAffinities = new Dictionary<string, float>();
		foreach (WrestlingMatchFinish finish in WrestlingMatchFinishManager.Instance.GetMatchFinishes()) {
			matchFinishAffinities.Add(finish.finishName, Random.Range (0.1f, 1f));
		}

		venue.Initialize(name, description, baseCost, gatePercentage, capacity, popularity, matchTypeAffinities, matchFinishAffinities, phase, mostPopularMatchTypeName);
	}
}
