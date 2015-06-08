using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SavedGame {
	public string gameID;

	public SavedGame() { 
		gameID = SavedGameManager.NewGameID;
	}

	public SavedGame(string gameID) {
		this.gameID = gameID;
	}
}

public class SavedGameManager : MonoBehaviour {
	public static string NewGameID = "new";
	
	List<SavedGame> games = new List<SavedGame>();
	SavedGame currentGame = null;

	public static SavedGameManager Instance;

	public static string SavedGameFilename {
		get { return "saved.games"; }
	}

	public string CurrentGameID {
		get { return currentGame.gameID; }
	}
	
	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

			LoadSavedGameInfo();
		}
		else {
			Destroy (gameObject);
		}
	}

	void LoadSavedGameInfo() {
		if (ES2.Exists(SavedGameFilename)) {
			string[] tags = ES2.GetTags(SavedGameFilename);
			foreach (string tag in tags) {
				string gameLocation = SavedGameFilename + "?tag=" + tag;
				SavedGame game = new SavedGame();
				game = ES2.Load<SavedGame>(gameLocation);
				games.Add (game);
			}
		}
	}

	public SavedGame CreateNewGame() {
		// Generate a unique ID for new games who are saving for the first time.
		int gameID = 0;
		while (ES2.Exists (SavedGameFilename + "?tag=" + gameID.ToString())) {
			gameID++;
		}
		currentGame = new SavedGame(gameID.ToString());
		SaveGame();
		games.Add(currentGame);
		return currentGame;
	}

	public void ClearSavedGames() {
		games.Clear();
	
		ES2.Delete (SavedGameFilename);

		ClearCurrentGame();
	}

	public List<SavedGame> GetSavedGames() {
		return games;
	}

	public void SetCurrentGame(SavedGame game) {
		currentGame = game;
	}

	public void ClearCurrentGame() {
		// Cleared saved data using the current game ID.

		// @TODO Replace with listener implementation instead of hardcoded objects.
		if (GameManager.Instance != null) {
			GameManager.Instance.ClearSavedData();
		}

		if (CompanyManager.Instance != null) {
			CompanyManager.Instance.ClearSavedData();
		}

		if (VenueManager.Instance != null) {
			VenueManager.Instance.ClearSavedData();
		}

		if (WrestlerManager.Instance != null) {
			WrestlerManager.Instance.ClearSavedData();
		}

		// Generate a new game ID.
		currentGame = new SavedGame();
	}

	public void SaveGame() {
		ES2.Save<SavedGame>(currentGame, SavedGameFilename + "?tag=" + currentGame.gameID);

		// @TODO Replace with listener implementation instead of hardcoded objects.
		if (GameManager.Instance != null) {
			GameManager.Instance.SaveData();
		}
		
		if (CompanyManager.Instance != null) {
			CompanyManager.Instance.SaveData();
		}
		
		if (VenueManager.Instance != null) {
			VenueManager.Instance.SaveData();
		}
		
		if (WrestlerManager.Instance != null) {
			WrestlerManager.Instance.SaveData();
		}
	}
}