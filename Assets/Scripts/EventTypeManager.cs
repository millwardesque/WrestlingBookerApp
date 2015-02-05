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
		EventType type;

		type = Instantiate(typePrefab) as EventType;
		type.Initialize("House show", "A basic house show. No cameras, just local butts in seats.", 2000.0f);
		types.Add (type);

		type = Instantiate(typePrefab) as EventType;
		type.Initialize("TV", "A TV show. Gets some national viewership.", 20000.0f);
		types.Add (type);

		type = Instantiate(typePrefab) as EventType;
		type.Initialize("PPV", "A pay-per-view show. Draws a world-wide audience.", 100000.0f);
		types.Add (type);
	}
	
	public List<EventType> GetTypes() {
		return types;
	}
}
