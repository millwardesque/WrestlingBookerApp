using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (Venue))]
public class VenueEditor : Editor {
	
	public override void OnInspectorGUI() {
		Venue venueTarget = target as Venue;
		DrawDefaultInspector();

		EditorGUILayout.LabelField("Match Type Preferences", venueTarget.matchTypePreferences.Count.ToString());
		foreach (string typeName in venueTarget.matchTypePreferences.Keys) {
			EditorGUILayout.LabelField("MT: " + typeName, venueTarget.matchTypePreferences[typeName].ToString());
		}

		EditorGUILayout.LabelField("Match Finish Preferences", venueTarget.matchFinishPreferences.Count.ToString());
		foreach (string finishName in venueTarget.matchFinishPreferences.Keys) {
			EditorGUILayout.LabelField("MF: " + finishName, venueTarget.matchFinishPreferences[finishName].ToString());
		}
	}
}