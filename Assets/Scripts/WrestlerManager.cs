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
				CreateWrestler(name, description, perMatchCost, popularity, isHeel, hiringCost);
			}
		}
		else {
			Debug.LogError("Unable to load wrestler data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}
	
	public List<Wrestler> GetWrestlers() {
		return wrestlers;
	}

	public Wrestler GetWrestler(string name) {
		return wrestlers.Find( x => x.wrestlerName == name );
	}
	
	public Wrestler CreateWrestler(string name, string description, float perMatchCost, float popularity, bool isHeel, float hiringCost) {
		Wrestler wrestler = Instantiate(wrestlerPrefab) as Wrestler;
		wrestler.transform.SetParent(transform, false);
		wrestler.Initialize(name, description, perMatchCost, popularity, isHeel, hiringCost);
		wrestlers.Add (wrestler);
		return wrestler;
	}
}
