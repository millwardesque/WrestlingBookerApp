using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WrestlingMatchFinishManager : MonoBehaviour {
	List<WrestlingMatchFinish> matchFinishes = new List<WrestlingMatchFinish>();
	
	// Use this for initialization
	void Start () {
		LoadFromJSON("matchFinishes");
	}

	/// <summary>
	///  Loads the event types from a JSON file.
	/// </summary>
	/// <param name="filename">Filename.</param>
	void LoadFromJSON(string filename) {
		TextAsset jsonAsset = Resources.Load<TextAsset>(filename);
		if (jsonAsset != null) {
			string fileContents = jsonAsset.text;
			var N = JSON.Parse(fileContents);
			var finishArray = N["match_finishes"].AsArray;
			foreach (JSONNode finish in finishArray) {
				string name = finish["name"];
				string description = finish["description"];
				CreateWrestlingMatchFinish(name, description);
			}
		}
		else {
			Debug.LogError("Unable to load event type data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}
	
	public List<WrestlingMatchFinish> GetMatchFinishes() {
		return matchFinishes;
	}
	
	public WrestlingMatchFinish CreateWrestlingMatchFinish(string name, string description) {
		WrestlingMatchFinish matchFinish = new WrestlingMatchFinish();
		matchFinish.Initialize(name, description);
		matchFinishes.Add (matchFinish);
		return matchFinish;
	}
}
