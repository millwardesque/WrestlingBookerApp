using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wrestler : MonoBehaviour {
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

	public void Initialize(string wrestlerName, string description, float perMatchCost, float popularity, bool isHeel, float hiringCost, int phase, float charisma, float work, float appearance, Dictionary<string, float> matchTypeAffinities) {
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
}
