using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WrestlerManager : MonoBehaviour {
	public List<Wrestler> wrestlers = new List<Wrestler>();
	public Wrestler wrestlerPrefab;
	WrestlerGenerator wrestlerGenerator = new WrestlerGenerator();

	public static WrestlerManager Instance;

	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);

			if (!wrestlerPrefab) {
				Debug.LogError("Unable to start Wrestler Manager: No wrestler prefab is set.");
			}
		}
		else {
			Destroy(gameObject);
		}
	}

	public string GetFilename(string gameID) {
		return gameID + ".wrestlers";
	}

	public void Save(string gameID) {
		foreach (Wrestler wrestler in wrestlers) {
			Wrestler.Save(wrestler, gameID);
		}
	}
	
	public void Load(string gameID) {
		DestroyCurrentGameObjects();
		
		string filename = GetFilename(gameID);
		if (ES2.Exists(filename)) {
			string[] tags = ES2.GetTags(filename);
			foreach (string tag in tags) {
				Wrestler wrestler = CreateEmptyWrestler();
				Wrestler.Load(wrestler, tag, gameID);
				wrestlers.Add (wrestler);
			}
		}
	}
	
	public void DeleteSaved(string gameID) {
		string filename = GetFilename(gameID);
		if (ES2.Exists(filename)) {
			ES2.Delete(filename);
		}
		
		if (gameID == SavedGameManager.Instance.CurrentGameID) {
			DestroyCurrentGameObjects();
		}
	}
	
	public void CreateNew() {
		int[] phaseCounts = new int[4];
		phaseCounts[0] = 6;
		phaseCounts[1] = 4;
		phaseCounts[2] = 4;
		phaseCounts[3] = 4;
		
		wrestlerGenerator.Initialize("wrestler-names");
		
		for (int phase = 0; phase < phaseCounts.Length; ++phase) {
			for (int i = 0; i < phaseCounts[phase]; ++i) {
				GenerateNewWrestler(phase);
			}
		}
	}
	
	void DestroyCurrentGameObjects() {
		foreach (Wrestler wrestler in wrestlers) {
			Destroy(wrestler.gameObject);
		}
		wrestlers.Clear();
	}

	public List<Wrestler> GetWrestlers(int phase = 0) {
		return wrestlers.FindAll( x => x.phase <= phase);
	}

	public Wrestler GetWrestler(string name) {
		return wrestlers.Find( x => x.wrestlerName == name );
	}

	public Wrestler CreateEmptyWrestler() {
		Wrestler wrestler = Instantiate(wrestlerPrefab) as Wrestler;
		wrestler.transform.SetParent(transform, false);
		return wrestler;
	}

	void GenerateNewWrestler(int phase) {
		Wrestler wrestler = CreateEmptyWrestler();
		wrestlerGenerator.GenerateWrestler(wrestler, phase);
		wrestlers.Add (wrestler);
	}
}
