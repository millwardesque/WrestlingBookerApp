using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VenueGenerator {
	List<Vector2> baseCostRange = new List<Vector2>();
	List<Vector2> gatePercentageRange = new List<Vector2>();
	List<Vector2> capacityRange = new List<Vector2>();
	
	public void Initialize() {
		baseCostRange.Clear();
		baseCostRange.Add (new Vector2(1000, 2000));
		baseCostRange.Add (new Vector2(2000, 15000));
		baseCostRange.Add (new Vector2(25000, 100000));
		baseCostRange.Add (new Vector2(1000000, 2000000));
		
		gatePercentageRange.Clear();
		gatePercentageRange.Add (new Vector2(0f, 0.2f));
		gatePercentageRange.Add (new Vector2(0f, 0.2f));
		gatePercentageRange.Add (new Vector2(0f, 0.2f));
		gatePercentageRange.Add (new Vector2(0f, 0.2f));

		capacityRange.Clear();
		capacityRange.Add (new Vector2(500, 500));
		capacityRange.Add (new Vector2(500, 2500));
		capacityRange.Add (new Vector2(2500, 10000));
		capacityRange.Add (new Vector2(15000, 100000));
	}
	
	public void GenerateVenue(Venue venue, int phase) {
		string name = venue.venueName;
		string description = venue.venueDescription;
		float baseCost = RandomRange(baseCostRange[phase]);
		float gatePercentage = RandomRange(gatePercentageRange[phase]);
		int capacity = RandomRangeInt(capacityRange[phase]);
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
	
	int RandomRangeInt(Vector2 range) {
		return Random.Range ((int)range.x, (int)range.y);
	}
	
	float RandomRange(Vector2 range) {
		return Random.Range (range.x, range.y);
	}
}
