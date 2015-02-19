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

	public string DescriptionWithStats {
		get {
			return string.Format ("Match Cost: ${0}\nWork: {1}\nCharisma: {2}\n{3}", perMatchCost, Utilities.AlphaRating(work), Utilities.AlphaRating(charisma), description);
		}
	}

	public void LoadAugmentedStats() {
		string prefix = "wrestlerChanges-" + wrestlerName;
		if (PlayerPrefs.HasKey(prefix + ".popularity")) {
			popularity = PlayerPrefs.GetFloat(prefix + ".popularity");
		}
		
		if (PlayerPrefs.HasKey(prefix + ".charisma")) {
			charisma = PlayerPrefs.GetFloat(prefix + ".charisma");
		}
		
		if (PlayerPrefs.HasKey(prefix + ".work")) {
			work = PlayerPrefs.GetFloat(prefix + ".work");
		}
		
		if (PlayerPrefs.HasKey(prefix + ".appearance")) {
			appearance = PlayerPrefs.GetFloat(prefix + ".appearance");
		}
		
		if (PlayerPrefs.HasKey(prefix + ".perMatchCost")) {
			perMatchCost = PlayerPrefs.GetFloat(prefix + ".perMatchCost");
		}
	}
	
	public void SaveAugmentedStats() {
		string prefix = "wrestlerChanges-" + wrestlerName;
		PlayerPrefs.SetFloat(prefix + ".popularity", popularity);
		PlayerPrefs.SetFloat(prefix + ".charisma", charisma);
		PlayerPrefs.SetFloat(prefix + ".work", work);
		PlayerPrefs.SetFloat(prefix + ".appearance", appearance);
		PlayerPrefs.SetFloat(prefix + ".perMatchCost", perMatchCost);
	}

	public void DeleteAugmentedStats() {
		string prefix = "wrestlerChanges-" + wrestlerName;
		PlayerPrefs.DeleteKey(prefix + ".popularity");
		PlayerPrefs.DeleteKey(prefix + ".charisma");
		PlayerPrefs.DeleteKey(prefix + ".work");
		PlayerPrefs.DeleteKey(prefix + ".appearance");
		PlayerPrefs.DeleteKey(prefix + ".perMatchCost");
	}
}
