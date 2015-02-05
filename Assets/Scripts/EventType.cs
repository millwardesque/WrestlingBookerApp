using UnityEngine;
using System.Collections;

public class EventType : MonoBehaviour {
	public string typeName;
	public string description;
	public float cost;
	
	public void Initialize(string typeName, string description, float cost) {
		this.typeName = typeName;
		this.description = description;
		this.cost = cost;
	}
}
