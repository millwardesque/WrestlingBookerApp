using UnityEngine;
using System.Collections;

public class Wrestler : MonoBehaviour {
	public string wrestlerName;
	public string description;
	public float perMatchCost;
	public float popularity;
	public bool isHeel;
	public float hiringCost;
	public int tier;

	public void Initialize(string wrestlerName, string description, float perMatchCost, float popularity, bool isHeel, float hiringCost, int tier) {
		this.wrestlerName = wrestlerName;
		this.description = description;
		this.perMatchCost = perMatchCost;
		this.popularity = popularity;
		this.isHeel = isHeel;
		this.hiringCost = hiringCost;
		this.tier = tier;
	}
}
