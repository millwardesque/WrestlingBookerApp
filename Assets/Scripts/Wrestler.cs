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
	public List<string> usedMatchTypes = new List<string>();

	public void Initialize(string wrestlerName, string description, float perMatchCost, float popularity, bool isHeel, float hiringCost, int phase, float charisma, float work, float appearance, Dictionary<string, float> matchTypeAffinities) {
		this.id = GetInstanceID().ToString();
		this.wrestlerName = wrestlerName;
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
	}

	public float GetMatchTypeAffinity(WrestlingMatchType matchType) {
		if (matchTypeAffinities.ContainsKey(matchType.typeName)) {
			return matchTypeAffinities[matchType.typeName];
		}
		else {
			return 0.5f;
		}
	}

	public string DescriptionWithStats {
		get {
			return string.Format ("Match Cost: ${0}\nWork: {1}\nCharisma: {2}\n{3}", perMatchCost, Utilities.AlphaRating(work), Utilities.AlphaRating(charisma), description);
		}
	}

	public void AddUsedMatchType(WrestlingMatchType type) {
		if (!HasUsedMatchType(type)) {
			usedMatchTypes.Add(type.typeName);
			Save ();
		}
	}
	
	public bool HasUsedMatchType(WrestlingMatchType type) {
		return (null != usedMatchTypes.Find ( x => x == type.typeName));
	}

	public void Save() {
		string wrestlerLocation = WrestlerManager.Instance.WrestlerFilename + "?tag=" + id;
		ES2.Save(this, wrestlerLocation);
	}
}
