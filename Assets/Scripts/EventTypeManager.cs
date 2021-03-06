﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class EventTypeManager : MonoBehaviour {
	List<EventType> types = new List<EventType>();
	public EventType typePrefab;
	
	// Use this for initialization
	void Start () {
		if (!typePrefab) {
			Debug.LogError("Unable to start Event Type Manager: No event type prefab is set.");
		}
	
		LoadFromJSON("eventTypes");
	}

	/// <summary>
	///  Loads the event types from a JSON file.
	/// </summary>
	/// <param name="filename">Filename.</param>
	void LoadFromJSON(string filename) {
		TextAsset jsonAsset = Resources.Load<TextAsset>(filename);
		if (jsonAsset != null) {
			string fileContents = jsonAsset.text;
			var N = JSON.Parse(fileContents);
			var eventTypeArray = N["event_types"].AsArray;
			foreach (JSONNode eventType in eventTypeArray) {
				string name = eventType["name"];
				string description = eventType["description"];
				float cost = eventType["cost"].AsFloat;
				float externalRevenuePerUser = eventType["externalRevenuePerUser"].AsFloat;
				float ticketsToExternalMultiplier = eventType["ticketsToExternalMultiplier"].AsFloat;
				int phase = eventType["phase"].AsInt;

				CreateEventType(name, description, cost, externalRevenuePerUser, ticketsToExternalMultiplier, phase);
			}
		}
		else {
			Debug.LogError("Unable to load event type data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}
	
	public List<EventType> GetTypes(int phase) {
		return types.FindAll( x => x.phase <= phase);
	}

	public EventType CreateEventType(string name, string description, float cost, float externalRevenuePerUser, float ticketsToExternalMultiplier, int phase) {
		EventType type = Instantiate(typePrefab) as EventType;
		type.transform.SetParent(transform, false);
		type.Initialize(name, description, cost, externalRevenuePerUser, ticketsToExternalMultiplier, phase);
		types.Add (type);
		return type;
	}
}
