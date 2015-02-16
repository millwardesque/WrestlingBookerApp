using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (Company))]
public class CompanyEditor : Editor {

	public override void OnInspectorGUI() {
		Company companyTarget = target as Company;
		DrawDefaultInspector();
		EditorGUILayout.LabelField("Popularity", companyTarget.Popularity.ToString());
	}
}
