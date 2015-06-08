using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (SavedGameManager))]
public class SavedGameManagerEditor : Editor {
	
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		
		SavedGameManager manager = target as SavedGameManager;
		EditorGUILayout.LabelField("Current Game", manager.CurrentGameID.ToString());
		EditorGUILayout.LabelField("Saved Games", manager.GetSavedGames().Count.ToString());
		foreach (SavedGame game in manager.GetSavedGames()) {
			EditorGUILayout.LabelField("ID: ", game.gameID.ToString());
		}
	}
}
