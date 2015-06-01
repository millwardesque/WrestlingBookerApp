using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (TrainingManager))]
public class TrainingManagerEditor : Editor {
	
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		TrainingManager manager = target as TrainingManager;
		EditorGUILayout.LabelField("Training Types", manager.TrainingOptions.Count.ToString());
		foreach (Training training in manager.TrainingOptions) {
			EditorGUILayout.LabelField("TT: " + training.trainingName, training.ToString());
		}
	}
}
