﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WrestlerGenerator {
	List<string> givenNames = new List<string>();
	List<string> surnames = new List<string>();
	List<string> nicknames = new List<string>();

	List<Vector2> perMatchCostRange = new List<Vector2>();
	List<Vector2> statRange = new List<Vector2>();
	List<Vector2> hiringCostRange = new List<Vector2>();

	
	public void Initialize(string wrestlerNameFilename) {
		LoadWrestlerNames(wrestlerNameFilename);

		perMatchCostRange.Clear();
		perMatchCostRange.Add (new Vector2(500, 2500));
		perMatchCostRange.Add (new Vector2(7500, 10000));
		perMatchCostRange.Add (new Vector2(12500, 50000));
		perMatchCostRange.Add (new Vector2(50000, 100000));

		statRange.Clear();
		statRange.Add (new Vector2(0.1f, 0.4f));
		statRange.Add (new Vector2(0.2f, 0.6f));
		statRange.Add (new Vector2(0.4f, 0.8f));
		statRange.Add (new Vector2(0.6f, 0.9f));

		hiringCostRange.Clear();
		hiringCostRange.Add (new Vector2(750, 1500));
		hiringCostRange.Add (new Vector2(2000, 20000));
		hiringCostRange.Add (new Vector2(35000, 100000));
		hiringCostRange.Add (new Vector2(125000, 500000));
	}

	public void GenerateWrestler(Wrestler wrestler, int phase) {
		string name = GenerateName();
		string description = "";	// @TODO: Decide if this is still needed.
		float perMatchCost = (float)RandomRangeInt(perMatchCostRange[phase]);
		float popularity = RandomRange(statRange[phase]);
		bool isHeel = (Random.Range (0, 2)) == 0 ? true : false;
		float hiringCost = (float)RandomRangeInt(hiringCostRange[phase]);
		float charisma = RandomRange(statRange[phase]);
		float work = RandomRange(statRange[phase]);
		float appearance = RandomRange(statRange[phase]);
		
		Dictionary<string, float> matchTypeAffinities = new Dictionary<string, float>();
		// @TODO Generate these from match types defined in the resource file.
		
		wrestler.Initialize(name, description, perMatchCost, popularity, isHeel, hiringCost, phase, charisma, work, appearance, matchTypeAffinities);
	}

	int RandomRangeInt(Vector2 range) {
		return Random.Range ((int)range.x, (int)range.y);
	}

	float RandomRange(Vector2 range) {
		return Random.Range (range.x, range.y);
	}

	void LoadWrestlerNames(string filename) {
		givenNames = new List<string>();
		surnames = new List<string>();
		nicknames = new List<string>();
		
		TextAsset jsonAsset = Resources.Load<TextAsset>(filename);
		if (jsonAsset != null) {
			string fileContents = jsonAsset.text;
			var N = JSON.Parse(fileContents);
			
			var nameArray = N["given"].AsArray;
			foreach (JSONNode name in nameArray) {
				givenNames.Add (name);
			}
			
			nameArray = N["surname"].AsArray;
			foreach (JSONNode name in nameArray) {
				surnames.Add (name);
			}
			
			nameArray = N["nickname"].AsArray;
			foreach (JSONNode name in nameArray) {
				nicknames.Add (name);
			}
		}
		else {
			Debug.LogError("Unable to load wrestler-name data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}

	string GenerateName() {
		// Probabilities for the type of name to generate. Should add up to one to make the actual probability match the percentages, but not required.
		float twoNameProb = 0.5f;
		float twoPlusNicknameProb = 0.25f;
		float justNicknameProb = 0.25f;
		float totalProb = twoNameProb + twoPlusNicknameProb + justNicknameProb;
		float rand = Random.Range(0, totalProb);
		
		if (rand < twoNameProb) {
			int firstIndex = Random.Range(0, givenNames.Count);
			int lastIndex = Random.Range(0, surnames.Count);
			return givenNames[firstIndex] + " " + surnames[lastIndex];
		}
		else {
			rand -= twoNameProb;
		}
		
		if (rand < twoPlusNicknameProb) {
			int firstIndex = Random.Range(0, givenNames.Count);
			int lastIndex = Random.Range(0, surnames.Count);
			int nicknameIndex = Random.Range(0, nicknames.Count);
			return givenNames[firstIndex] + " '" + nicknames[nicknameIndex] + "' " + surnames[lastIndex];
		}
		else {
			rand -= twoPlusNicknameProb;
		}
		
		if (rand < justNicknameProb) {
			int nicknameIndex = Random.Range(0, nicknames.Count);
			return nicknames[nicknameIndex];
		}
		else {
			Debug.LogError ("Unable to generator a user. Probability " + rand + " was greater than the available options somehow.");
		}
		return "<Unknown>";
	}
}
