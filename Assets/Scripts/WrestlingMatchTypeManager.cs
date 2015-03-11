using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WrestlingMatchTypeManager : MonoBehaviour {
	List<WrestlingMatchType> matchTypes = new List<WrestlingMatchType>();

	public static WrestlingMatchTypeManager Instance;

	// Use this for initialization
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);

			LoadFromJSON("matchTypes");
		}
		else {
			Destroy(gameObject);
		}
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
				int phase = matchType["phase"].AsInt;
				CreateWrestlingMatchType(name, description, phase);
			}
		}
		else {
			Debug.LogError("Unable to load event type data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}

	public WrestlingMatchType GetMatchType(string name) {
		return matchTypes.Find ( x => x.typeName == name );
	}
	
	public List<WrestlingMatchType> GetMatchTypes(int phase) {
		return matchTypes.FindAll( x => x.phase <= phase);
	}

	public List<WrestlingMatchType> GetMatchTypes() {
		return matchTypes;
	}

	
	public WrestlingMatchType CreateWrestlingMatchType(string name, string description, int phase) {
		WrestlingMatchType matchType = new WrestlingMatchType();
		matchType.Initialize(name, description, phase);
		matchTypes.Add (matchType);
		return matchType;
	}
}
