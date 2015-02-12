using UnityEngine;
using System.Collections;

public class EventType : MonoBehaviour {
	public string typeName;
	public string description;
	public float cost;
	public float externalRevenuePerUser = 0.0f;
	public float ticketsToExternalMultiplier = 1.0f;
	
	public void Initialize(string typeName, string description, float cost, float externalRevenuePerUser, float ticketsToExternalMultiplier) {
		this.typeName = typeName;
		this.description = description;
		this.cost = cost;
		this.externalRevenuePerUser = externalRevenuePerUser;
		this.ticketsToExternalMultiplier = ticketsToExternalMultiplier;
	}
}
