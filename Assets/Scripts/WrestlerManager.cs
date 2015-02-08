using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WrestlerManager : MonoBehaviour {
	List<Wrestler> wrestlers = new List<Wrestler>();
	public Wrestler wrestlerPrefab;
	
	// Use this for initialization
	void Start () {
		if (!wrestlerPrefab) {
			Debug.LogError("Unable to start Wrestler Manager: No wrestler prefab is set.");
		}

		LoadFromJSON("Wrestlers");
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
				CreateWrestler(name, description, perMatchCost);
			}
		}
		else {
			Debug.LogError("Unable to load wrestler data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}
	
	public List<Wrestler> GetWrestlers() {
		return wrestlers;
	}
	
	public Wrestler CreateWrestler(string name, string description, float perMatchCost) {
		Wrestler wrestler = Instantiate(wrestlerPrefab) as Wrestler;
		wrestler.transform.SetParent(transform, false);
		wrestler.Initialize(name, description, perMatchCost);
		wrestlers.Add (wrestler);
		return wrestler;
	}
}
