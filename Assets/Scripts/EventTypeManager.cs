using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventTypeManager : MonoBehaviour {
	List<EventType> types;
	public EventType typePrefab;
	
	// Use this for initialization
	void Start () {
		if (!typePrefab) {
			Debug.LogError("Unable to start Event Type Manager: No event type prefab is set.");
		}
		
		types = new List<EventType>();
		
		// @TODO Load these from an external source.
		CreateEventType("House show", "A basic house show. No cameras, just local butts in seats.", 2000.0f);
		CreateEventType("TV", "A TV show. Gets some national viewership.", 20000.0f);
		CreateEventType("PPV", "A pay-per-view show. Draws a world-wide audience.", 100000.0f);
	}
	
	public List<EventType> GetTypes() {
		return types;
	}

	public EventType CreateEventType(string name, string description, float cost) {
		EventType type = Instantiate(typePrefab) as EventType;
		type.transform.SetParent(transform, false);
		type.Initialize(name, description, cost);
		types.Add (type);
		return type;
	}
}
