using UnityEngine;
using System.Collections;

public class Wrestler : MonoBehaviour {
	public string wrestlerName;
	public string description;
	public float perMatchCost;

	public void Initialize(string wrestlerName, string description, float perMatchCost) {
		this.wrestlerName = wrestlerName;
		this.description = description;
		this.perMatchCost = perMatchCost;
	}
}
