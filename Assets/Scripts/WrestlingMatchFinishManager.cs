using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WrestlingMatchFinishManager : MonoBehaviour {
	List<WrestlingMatchFinish> matchFinishes = new List<WrestlingMatchFinish>();

	public List<WrestlingMatchFinish> MatchFinishes {
		get { return matchFinishes; }
	}

	public static WrestlingMatchFinishManager Instance;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);

			LoadFromJSON("matchFinishes");
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
			var finishArray = N["match_finishes"].AsArray;
			foreach (JSONNode finish in finishArray) {
				string name = finish["name"];
				string description = finish["description"];
				int phase = finish["phase"].AsInt;
				CreateWrestlingMatchFinish(name, description, phase);
			}
		}
		else {
			Debug.LogError("Unable to load event type data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}

	public List<WrestlingMatchFinish> GetMatchFinishes() {
		return matchFinishes;
	}

	public List<WrestlingMatchFinish> GetMatchFinishes(int phase) {
		return matchFinishes.FindAll( x => x.phase <= phase);
	}
	
	public WrestlingMatchFinish CreateWrestlingMatchFinish(string name, string description, int phase) {
		WrestlingMatchFinish matchFinish = new WrestlingMatchFinish();
		matchFinish.Initialize(name, description, phase);
		matchFinishes.Add (matchFinish);
		return matchFinish;
	}
}
