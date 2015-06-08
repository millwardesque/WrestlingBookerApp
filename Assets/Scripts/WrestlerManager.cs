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

	public void LoadWrestlers() {
		Debug.Log (WrestlerFilename);
		if (!LoadSavedWrestlers()) {
			GenerateNewWrestlers();
		}
	}

	public string WrestlerFilename {
		get { return SavedGameManager.Instance.CurrentGameID + ".wrestlers"; }
	}

	bool LoadSavedWrestlers() {
		if (ES2.Exists(WrestlerFilename)) {
			string[] tags = ES2.GetTags(WrestlerFilename);
			foreach (string tag in tags) {
				string wrestlerLocation = WrestlerFilename + "?tag=" + tag;
				Wrestler wrestler = Instantiate(wrestlerPrefab) as Wrestler;
				wrestler.transform.SetParent(transform, false);
				ES2.Load<Wrestler>(wrestlerLocation, wrestler);
				wrestler.name = wrestler.wrestlerName;
				wrestlers.Add (wrestler);
			}
			return true;
		}
		else {
			return false;
		}
	}

	public void GenerateNewWrestlers() {
		int[] phaseCounts = new int[4];
		phaseCounts[0] = 6;
		phaseCounts[1] = 4;
		phaseCounts[2] = 4;
		phaseCounts[3] = 4;

		// Clean up existing data.
		foreach (Wrestler wrestler in wrestlers) {
			Destroy(wrestler.gameObject);
		}
		wrestlers.Clear();

		if (ES2.Exists(WrestlerFilename)) {
			ES2.Delete (WrestlerFilename);
		}

		wrestlerGenerator.Initialize("wrestler-names");

		for (int phase = 0; phase < phaseCounts.Length; ++phase) {
			for (int i = 0; i < phaseCounts[phase]; ++i) {
				GenerateNewWrestler(phase);
			}
		}
	}

	public List<Wrestler> GetWrestlers(int phase = 0) {
		return wrestlers.FindAll( x => x.phase <= phase);
	}

	public Wrestler GetWrestler(string name) {
		return wrestlers.Find( x => x.wrestlerName == name );
	}

	public void ClearSavedData() {
		GenerateNewWrestlers();
	}

	public void SaveData() {
		foreach (Wrestler wrestler in wrestlers) {
			wrestler.Save();
		}
	}
	
	public void GenerateNewWrestler(int phase) {
		Wrestler wrestler = Instantiate(wrestlerPrefab) as Wrestler;
		wrestler.transform.SetParent(transform, false);
		wrestlerGenerator.GenerateWrestler(wrestler, phase);
		wrestler.name = wrestler.wrestlerName;
		wrestlers.Add (wrestler);
	}
}
