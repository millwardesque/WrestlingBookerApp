using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WrestlerGenerator {
	List<string> givenNames = new List<string>();
	List<string> surnames = new List<string>();
	List<string> nicknames = new List<string>();
	
	public void Initialize(string filename) {
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

	public void GenerateWrestler(Wrestler wrestler, int phase) {
		string name = GenerateName();
		string description = "";
		float perMatchCost = Random.Range (500f, 2500f);
		float popularity = Random.Range (0.1f, 0.4f);
		bool isHeel = (Random.Range (0, 2)) == 0 ? true : false;
		float hiringCost = Random.Range (750f, 1500f);
		float charisma = Random.Range (0.1f, 0.4f);
		float work = Random.Range (0.1f, 0.4f);
		float appearance = Random.Range (0.1f, 0.4f);
		
		Dictionary<string, float> matchTypeAffinities = new Dictionary<string, float>();
		// @TODO Generate these from match types defined in the resource file.
		
		wrestler.Initialize(name, description, perMatchCost, popularity, isHeel, hiringCost, phase, charisma, work, appearance, matchTypeAffinities);
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
