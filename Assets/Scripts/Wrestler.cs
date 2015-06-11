using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wrestler : MonoBehaviour {
	public string id;
	public string wrestlerName;
	public string description;
	public float perMatchCost;
	public float popularity;
	public bool isHeel;
	public float hiringCost;
	public int phase;
	public float charisma;
	public float work;
	public float appearance;
	public Dictionary<string, float> matchTypeAffinities;
	public Dictionary<string, float> matchFinishAffinities;
	public List<string> usedMatchTypes = new List<string>();
	public List<string> usedMatchFinishes = new List<string>();

	public void Initialize(string wrestlerName, string description, float perMatchCost, float popularity, bool isHeel, float hiringCost, int phase, float charisma, float work, float appearance, Dictionary<string, float> matchTypeAffinities, Dictionary<string, float> matchFinishAffinities) {
		this.id = GetInstanceID().ToString();
		this.wrestlerName = wrestlerName;
		this.name = wrestlerName;
		this.description = description;
		this.perMatchCost = perMatchCost;
		this.popularity = popularity;
		this.isHeel = isHeel;
		this.hiringCost = hiringCost;
		this.phase = phase;
		this.charisma = charisma;
		this.work = work;
		this.appearance = appearance;
		this.matchTypeAffinities = matchTypeAffinities;
		this.matchFinishAffinities = matchFinishAffinities;
	}

	public float GetMatchTypeAffinity(WrestlingMatchType matchType) {
		if (matchTypeAffinities.ContainsKey(matchType.typeName)) {
			return matchTypeAffinities[matchType.typeName];
		}
		else {
			return 0f;
		}
	}

	public float GetMatchFinishAffinity(WrestlingMatchFinish matchFinish) {
		if (matchFinishAffinities.ContainsKey(matchFinish.finishName)) {
			return matchFinishAffinities[matchFinish.finishName];
		}
		else {
			return 0f;
		}
	}

	public string DescriptionWithStats {
		get {
			return string.Format ("Per-match Cost: ${0}\nWork: {1}\nCharisma: {2}\nPopularity: {3}\nAppearance: {4}\nAlignment: {5}\n{6}", 
			                      perMatchCost, Utilities.FractionString(work, 10), Utilities.FractionString(charisma, 10), Utilities.FractionString(appearance, 10), Utilities.FractionString(popularity, 10), (isHeel ? "Heel" : "Babyface"), description);
		}
	}

	public void AddUsedMatchType(WrestlingMatchType type) {
		if (!HasUsedMatchType(type)) {
			usedMatchTypes.Add(type.typeName);
			Wrestler.Save (this, SavedGameManager.Instance.CurrentGameID);
		}
	}

	public void AddUsedMatchFinish(WrestlingMatchFinish finish) {
		if (!HasUsedMatchFinish(finish)) {
			usedMatchFinishes.Add(finish.finishName);
			Wrestler.Save (this, SavedGameManager.Instance.CurrentGameID);
		}
	}

	
	public bool HasUsedMatchType(WrestlingMatchType type) {
		return (null != usedMatchTypes.Find ( x => x == type.typeName));
	}

	public bool HasUsedMatchFinish(WrestlingMatchFinish finish) {
		return (null != usedMatchFinishes.Find ( x => x == finish.finishName));
	}

	public static bool Save(Wrestler wrestler, string gameID) {
		string filename = WrestlerManager.GetFilename(gameID) + "?tag=" + wrestler.id;
		ES2.Save(wrestler, filename);
		return true;
	}
	
	public static bool Load(Wrestler wrestler, string id, string gameID) {
		string filename = WrestlerManager.GetFilename(gameID) + "?tag=" + id;
		if (ES2.Exists(filename)) {
			ES2.Load<Wrestler>(filename, wrestler);
			wrestler.name = wrestler.wrestlerName;
			return true;
		}
		else {
			Debug.LogError(string.Format ("Unable to load wrestler from {0}: No such file found", filename));
			return false;
		}
	}
	
	public static bool DeleteSaved(string id, string gameID) {
		string filename = WrestlerManager.GetFilename(gameID) + "?tag=" + id;
		if (ES2.Exists(filename)) {
			ES2.Delete(filename);
			return true;
		}
		else {
			Debug.LogError(string.Format ("Unable to delete wrestler at {0}: No such file found", filename));
			return false;
		}
	}
}
