using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class TrainingManager : MonoBehaviour {
	List<Training> trainingOptions = new List<Training>();
	
	public List<Training> TrainingOptions {
		get { return trainingOptions; }
	}
	
	public static TrainingManager Instance;
	
	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
			
			LoadFromJSON("training");
		}
		else {
			Destroy(gameObject);
		}
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
			var array = N["training"].AsArray;
			foreach (JSONNode training in array) {
				string name = training["name"];
				string description = training["description"];
				float cost = training["cost"].AsFloat;
				int phase = training["phase"].AsInt;
				
				List<TrainingEffect> trainingEffects = new List<TrainingEffect>();
				var trainingEffectData = training["effects"].AsArray;
				for (int i = 0; i < trainingEffectData.Count; ++i) {
					JSONNode effectData = trainingEffectData[i];
					TrainingEffect effect = new TrainingEffect(effectData["attribute"], effectData["min"].AsFloat, effectData["max"].AsFloat);
					trainingEffects.Add(effect);
				}
				CreateTrainingOption(name, description, cost, trainingEffects, phase);
			}
		}
		else {
			Debug.LogError("Unable to load training data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}
	
	public List<Training> GetTrainingOptions() {
		return trainingOptions;
	}
	
	public List<Training> GetTrainingOptions(int phase) {
		return trainingOptions.FindAll( x => x.phase <= phase);
	}
	
	public Training CreateTrainingOption(string name, string description, float cost, List<TrainingEffect> trainingEffects, int phase) {
		Training trainingOption = new Training();
		trainingOption.Initialize(name, description, cost, trainingEffects, phase);
		trainingOptions.Add (trainingOption);
		return trainingOption;
	}
}
