using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

class WrestlerNameGenerator {
	List<string> givenNames = new List<string>();
	List<string> surnames = new List<string>();
	List<string> nicknames = new List<string>();

	public void LoadWrestlerNames(string filename) {
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

	public string GenerateName() {
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

public class WrestlerManager : MonoBehaviour {
	public List<Wrestler> wrestlers = new List<Wrestler>();
	public Wrestler wrestlerPrefab;
	WrestlerNameGenerator nameGenerator = new WrestlerNameGenerator();
	
	// Use this for initialization
	void Awake () {
		if (!wrestlerPrefab) {
			Debug.LogError("Unable to start Wrestler Manager: No wrestler prefab is set.");
		}

		string gameID = "1";

		if (!LoadSavedWrestlers(gameID)) {
			Debug.Log ("No existing wrestlers");
			GenerateNewWrestlers(gameID);
		}
	}

	bool LoadSavedWrestlers(string gameID) {
		string wrestlerFilename = gameID + ".wrestlers";

		if (ES2.Exists(wrestlerFilename)) {
			string[] tags = ES2.GetTags(wrestlerFilename);
			foreach (string tag in tags) {
				string wrestlerLocation = wrestlerFilename + "?tag=" + tag;
				Wrestler wrestler = Instantiate(wrestlerPrefab) as Wrestler;
				wrestler.transform.SetParent(transform, false);
				ES2.Load<Wrestler>(wrestlerLocation, wrestler);
				wrestlers.Add (wrestler);
			}
			return true;
		}
		else {
			return false;
		}
	}

	public void GenerateNewWrestlers(string gameID) {
		string wrestlerFilename = gameID + ".wrestlers";
		int phase0Count = 4;
		int phase1Count = 4;
		int phase2Count = 4;
		int phase3Count = 4;

		// Clean up existing data.
		foreach (Wrestler wrestler in wrestlers) {
			Destroy(wrestler.gameObject);
		}
		wrestlers.Clear();

		if (ES2.Exists(wrestlerFilename)) {
			ES2.Delete (wrestlerFilename);
		}

		nameGenerator.LoadWrestlerNames("wrestler-names");

		for (int i = 0; i < phase0Count; ++i) {
			string name = nameGenerator.GenerateName();
			string description = "";
			float perMatchCost = Random.Range (500f, 2500f);
			float popularity = Random.Range (0.1f, 0.4f);
			bool isHeel = (Random.Range (0, 2)) == 0 ? true : false;
			float hiringCost = Random.Range (750f, 1500f);
			int phase = 0;
			float charisma = Random.Range (0.1f, 0.4f);
			float work = Random.Range (0.1f, 0.4f);
			float appearance = Random.Range (0.1f, 0.4f);
			
			Dictionary<string, float> matchTypeAffinities = new Dictionary<string, float>();
			// @TODO Generate these from match types defined in the resource file.
			
			CreateWrestler(name, description, perMatchCost, popularity, isHeel, hiringCost, phase, charisma, work, appearance, matchTypeAffinities);
		}

		for (int i = 0; i < phase1Count; ++i) {
			string name = nameGenerator.GenerateName();
			string description = "";
			float perMatchCost = Random.Range (500f, 2500f);
			float popularity = Random.Range (0.1f, 0.4f);
			bool isHeel = (Random.Range (0, 2)) == 0 ? true : false;
			float hiringCost = Random.Range (750f, 1500f);
			int phase = 1;
			float charisma = Random.Range (0.1f, 0.4f);
			float work = Random.Range (0.1f, 0.4f);
			float appearance = Random.Range (0.1f, 0.4f);

			Dictionary<string, float> matchTypeAffinities = new Dictionary<string, float>();
			// @TODO Generate these from match types defined in the resource file.

			CreateWrestler(name, description, perMatchCost, popularity, isHeel, hiringCost, phase, charisma, work, appearance, matchTypeAffinities);
		}

		for (int i = 0; i < phase2Count; ++i) {
			string name = nameGenerator.GenerateName();
			string description = "";
			float perMatchCost = Random.Range (500f, 2500f);
			float popularity = Random.Range (0.1f, 0.4f);
			bool isHeel = (Random.Range (0, 2)) == 0 ? true : false;
			float hiringCost = Random.Range (750f, 1500f);
			int phase = 2;
			float charisma = Random.Range (0.1f, 0.4f);
			float work = Random.Range (0.1f, 0.4f);
			float appearance = Random.Range (0.1f, 0.4f);
			
			Dictionary<string, float> matchTypeAffinities = new Dictionary<string, float>();
			// @TODO Generate these from match types defined in the resource file.
			
			CreateWrestler(name, description, perMatchCost, popularity, isHeel, hiringCost, phase, charisma, work, appearance, matchTypeAffinities);
		}

		for (int i = 0; i < phase3Count; ++i) {
			string name = nameGenerator.GenerateName();
			string description = "";
			float perMatchCost = Random.Range (500f, 2500f);
			float popularity = Random.Range (0.1f, 0.4f);
			bool isHeel = (Random.Range (0, 2)) == 0 ? true : false;
			float hiringCost = Random.Range (750f, 1500f);
			int phase = 3;
			float charisma = Random.Range (0.1f, 0.4f);
			float work = Random.Range (0.1f, 0.4f);
			float appearance = Random.Range (0.1f, 0.4f);
			
			Dictionary<string, float> matchTypeAffinities = new Dictionary<string, float>();
			// @TODO Generate these from match types defined in the resource file.
			
			CreateWrestler(name, description, perMatchCost, popularity, isHeel, hiringCost, phase, charisma, work, appearance, matchTypeAffinities);
		}

		foreach (Wrestler wrestler in wrestlers) {
			string wrestlerLocation = wrestlerFilename + "?tag=" + wrestler.GetInstanceID();
			ES2.Save(wrestler, wrestlerLocation);
		}
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

	public void ClearSavedData(string gameID) {
		GenerateNewWrestlers(gameID);
	}
	
	public Wrestler CreateWrestler(string name, string description, float perMatchCost, float popularity, bool isHeel, float hiringCost, int phase, float charisma, float work, float appearance, Dictionary<string, float> matchTypeAffinities) {
		Wrestler wrestler = Instantiate(wrestlerPrefab) as Wrestler;
		wrestler.transform.SetParent(transform, false);
		wrestler.Initialize(name, description, perMatchCost, popularity, isHeel, hiringCost, phase, charisma, work, appearance, matchTypeAffinities);
		wrestlers.Add (wrestler);

		return wrestler;
	}
}
