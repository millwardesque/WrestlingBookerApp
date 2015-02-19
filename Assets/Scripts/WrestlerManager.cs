using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WrestlerManager : MonoBehaviour {
	List<Wrestler> wrestlers = new List<Wrestler>();
	public Wrestler wrestlerPrefab;
	
	// Use this for initialization
	void Awake () {
		if (!wrestlerPrefab) {
			Debug.LogError("Unable to start Wrestler Manager: No wrestler prefab is set.");
		}

		LoadFromJSON("wrestlers");
	}

	/// <summary>
	///  Loads the wrestlers from a JSON file.
	/// </summary>
	/// <param name="filename">Filename.</param>
	void LoadFromJSON(string filename) {
		TextAsset jsonAsset = Resources.Load<TextAsset>(filename);
		if (jsonAsset != null) {
			string fileContents = jsonAsset.text;
			var N = JSON.Parse(fileContents);
			var wrestlerArray = N["wrestlers"].AsArray;
			foreach (JSONNode wrestler in wrestlerArray) {
				string name = wrestler["name"];
				string description = wrestler["description"];
				float perMatchCost = wrestler["perMatchCost"].AsFloat;
				float popularity = wrestler["popularity"].AsFloat;
				bool isHeel = wrestler["isHeel"].AsBool;
				float hiringCost = wrestler["hiringCost"].AsFloat;
				int phase = wrestler["phase"].AsInt;
				float charisma = wrestler["charisma"].AsFloat;
				float work = wrestler["work"].AsFloat;
				float appearance = wrestler["appearance"].AsFloat;

				var matchTypeAffinityArray = wrestler["matchTypeAffinities"].AsArray;
				Dictionary<string, float> matchTypeAffinities = new Dictionary<string, float>();
				foreach (JSONNode type in matchTypeAffinityArray) {
					matchTypeAffinities.Add(type["name"], type["affinity"].AsFloat);
				}

				CreateWrestler(name, description, perMatchCost, popularity, isHeel, hiringCost, phase, charisma, work, appearance, matchTypeAffinities);
			}
		}
		else {
			Debug.LogError("Unable to load wrestler data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}

	public List<Wrestler> GetWrestlers(int phase = 0) {
		return wrestlers.FindAll( x => x.phase <= phase);
	}

	public Wrestler GetWrestler(string name) {
		return wrestlers.Find( x => x.wrestlerName == name );
	}
	
	public Wrestler CreateWrestler(string name, string description, float perMatchCost, float popularity, bool isHeel, float hiringCost, int phase, float charisma, float work, float appearance, Dictionary<string, float> matchTypeAffinities) {
		Wrestler wrestler = Instantiate(wrestlerPrefab) as Wrestler;
		wrestler.transform.SetParent(transform, false);
		wrestler.Initialize(name, description, perMatchCost, popularity, isHeel, hiringCost, phase, charisma, work, appearance, matchTypeAffinities);
		wrestler.LoadAugmentedStats();
		wrestlers.Add (wrestler);

		return wrestler;
	}
}
