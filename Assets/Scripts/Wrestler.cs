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
	
	List<string> usedMatchTypes = new List<string>();

	string StoragePrefix {
		get { return "wrestlerChanges-" + wrestlerName; }
	}


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

	public void AddUsedMatchType(WrestlingMatchType type) {
		if (!HasUsedMatchType(type)) {
			usedMatchTypes.Add(type.typeName);
			SaveAugmentedData();
		}
	}
	
	public bool HasUsedMatchType(WrestlingMatchType type) {
		return (null != usedMatchTypes.Find ( x => x == type.typeName));
	}

	public void LoadAugmentedData() {
		string prefix = StoragePrefix;
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

		if (PlayerPrefs.HasKey(StoragePrefix + ".usedMatchTypes")) {
			usedMatchTypes = new List<string>(PlayerPrefs.GetString(prefix + ".usedMatchTypes").Split(';'));
		}
	}
	
	public void SaveAugmentedData() {
		string prefix = StoragePrefix;
		PlayerPrefs.SetFloat(prefix + ".popularity", popularity);
		PlayerPrefs.SetFloat(prefix + ".charisma", charisma);
		PlayerPrefs.SetFloat(prefix + ".work", work);
		PlayerPrefs.SetFloat(prefix + ".appearance", appearance);
		PlayerPrefs.SetFloat(prefix + ".perMatchCost", perMatchCost);

		if (usedMatchTypes.Count > 0) {
			string usedMatchTypeNames = "";
			foreach (string matchType in usedMatchTypes) {
				usedMatchTypeNames += matchType + ";";
			}
			usedMatchTypeNames = usedMatchTypeNames.Substring(0, usedMatchTypeNames.Length - 1); // Remove the trailing separator
			PlayerPrefs.SetString(prefix + ".usedMatchTypes", usedMatchTypeNames);
		}
	}

	public void DeleteAugmentedData() {
		string prefix = StoragePrefix;

		// @TODO Reload / reset this data
		PlayerPrefs.DeleteKey(prefix + ".popularity");
		PlayerPrefs.DeleteKey(prefix + ".charisma");
		PlayerPrefs.DeleteKey(prefix + ".work");
		PlayerPrefs.DeleteKey(prefix + ".appearance");
		PlayerPrefs.DeleteKey(prefix + ".perMatchCost");

		PlayerPrefs.DeleteKey(prefix + ".usedMatchTypes");
		usedMatchTypes = new List<string>();
	}
}
