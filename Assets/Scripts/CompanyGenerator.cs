using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class CompanyGenerator {
	List<Vector2> moneyRange = new List<Vector2>();
	List<Vector2> popularityRange = new List<Vector2>();

	public void Initialize() {
		moneyRange.Clear();
		moneyRange.Add (new Vector2(3000, 20000));
		moneyRange.Add (new Vector2(18000, 180000));
		moneyRange.Add (new Vector2(150000, 500000));
		moneyRange.Add (new Vector2(500000, 10000000));
		
		popularityRange.Clear();
		popularityRange.Add (new Vector2(0.1f, 0.4f));
		popularityRange.Add (new Vector2(0.2f, 0.6f));
		popularityRange.Add (new Vector2(0.4f, 0.8f));
		popularityRange.Add (new Vector2(0.6f, 0.9f));
	}
	
	public void Generate(Company company, int phase) {
		string name = GenerateName();

		int maxRosterSize = MaxRosterSize(phase);
		float money = RandomRange (moneyRange[phase]);

		List<Wrestler> roster = new List<Wrestler>(); // @TODO Add wrestlers if necessary
		bool isInAlliance = (Random.Range (0, 2) == 0) ? true : false;

		company.Initialize(name, money, maxRosterSize, phase, roster, isInAlliance);

		// Since popularity is a calculated value and not a stored one, fudge it by creating a dummy event with the desired rating.
		float popularity = RandomRange (popularityRange[phase]);
		HistoricalWrestlingEvent wrestlingEvent = new HistoricalWrestlingEvent();
		wrestlingEvent.Initialize(name + "-event0", 0, 0, "<dummy>", "<dummy>", 0f, popularity);
		company.AddEvent(wrestlingEvent);
	}
	
	int RandomRangeInt(Vector2 range) {
		return Random.Range ((int)range.x, (int)range.y);
	}
	
	float RandomRange(Vector2 range) {
		return Random.Range (range.x, range.y);
	}
	
	string GenerateName() {
		// Probabilities for the type of name to generate. Should add up to one to make the actual probability match the percentages, but not required.
		float threeInitialProb = 0.75f;
		float fourInitialProb = 0.25f;
		float totalProb = threeInitialProb + fourInitialProb;
		float rand = Random.Range(0, totalProb);
		char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
		
		if (rand < threeInitialProb) {
			return string.Format ("{0}{1}{2}", letters[Random.Range(0, letters.Length)], letters[Random.Range(0, letters.Length)], letters[Random.Range(0, letters.Length)]);
		}
		else {
			rand -= threeInitialProb;
		}
		
		if (rand < fourInitialProb) {
			return string.Format ("{0}{1}{2}{3}", letters[Random.Range(0, letters.Length)], letters[Random.Range(0, letters.Length)], letters[Random.Range(0, letters.Length)], letters[Random.Range(0, letters.Length)]);
		}
		else {
			Debug.LogError ("Unable to generator a company name. Probability " + rand + " was greater than the available options somehow.");
		}

		return "<Unknown>";
	}

	int MaxRosterSize(int phase) {
		switch(phase) {
		case 0:
			return 4;
		case 1:
			return 6;
		case 2:
			return 8;
		case 3:
			return 10;
		default:
			return 10;
		}
	}
}
