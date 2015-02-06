using UnityEngine;
using System.Collections;

public class Wrestler : MonoBehaviour {
	public string wrestlerName;
	public string description;

	public void Initialize(string wrestlerName, string description) {
		this.wrestlerName = wrestlerName;
		this.description = description;
	}
}
