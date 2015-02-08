using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WrestlingMatchTypeManager : MonoBehaviour {
	List<WrestlingMatchType> matchTypes = new List<WrestlingMatchType>();

	// Use this for initialization
	void Start () {		
		LoadFromJSON("matchTypes");
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
			var matchTypeArray = N["match_types"].AsArray;
			foreach (JSONNode matchType in matchTypeArray) {
				string name = matchType["name"];
				string description = matchType["description"];
				CreateWrestlingMatchType(name, description);
			}
		}
		else {
			Debug.LogError("Unable to load event type data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}
	
	public List<WrestlingMatchType> GetMatchTypes() {
		return matchTypes;
	}
	
	public WrestlingMatchType CreateWrestlingMatchType(string name, string description) {
		WrestlingMatchType matchType = new WrestlingMatchType();
		matchType.Initialize(name, description);
		matchTypes.Add (matchType);
		return matchType;
	}
}
