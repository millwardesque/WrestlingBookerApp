using UnityEngine;
using System.Collections;

public class Wrestler : MonoBehaviour {
	public string wrestlerName;
	public string description;
	public float perMatchCost;
	public float popularity;
	public bool isHeel;
	public float hiringCost;
	public int phase;

	public void Initialize(string wrestlerName, string description, float perMatchCost, float popularity, bool isHeel, float hiringCost, int phase) {
		this.wrestlerName = wrestlerName;
		this.description = description;
		this.perMatchCost = perMatchCost;
		this.popularity = popularity;
		this.isHeel = isHeel;
		this.hiringCost = hiringCost;
		this.phase = phase;
	}
}
