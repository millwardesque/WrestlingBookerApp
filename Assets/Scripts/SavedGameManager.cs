using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SavedGame {
	public string gameID;

	public SavedGame() { 
		gameID = "new";
	}

	public SavedGame(string gameID) {
		this.gameID = gameID;
	}
}

public class SavedGameManager : MonoBehaviour {	
	List<SavedGame> games = new List<SavedGame>();
	SavedGame currentGame = null;

	public static SavedGameManager Instance;

	public static string SavedGameFilename {
		get { return "saved.games"; }
	}

	public string CurrentGameID {
		get { return currentGame.gameID; }
	}

	public bool IsGameLoaded() {
		return (currentGame == null);
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

	public List<SavedGame> GetSavedGames() {
		return games;
	}

	public SavedGame GetSavedGame(string gameID) {
		return games.Find (x => x.gameID == gameID);
	}

	public SavedGame CreateNewGame() {
		// Generate a unique ID for new games who are saving for the first time.
		int gameID = 0;
		while (ES2.Exists (SavedGameFilename + "?tag=" + gameID.ToString())) {
			gameID++;
		}
		currentGame = new SavedGame(gameID.ToString());

		if (VenueManager.Instance != null) {
			VenueManager.Instance.CreateNew();
		}

		if (WrestlerManager.Instance != null) {
			WrestlerManager.Instance.CreateNew();
		}

		if (CompanyManager.Instance != null) {
			CompanyManager.Instance.CreateNew();
		}

		if (GameManager.Instance != null) {
			GameManager.Instance.CreateNew();
		}

		Save ();
		games.Add(currentGame);
		return currentGame;
	}

	public void Save() {
		ES2.Save<SavedGame>(currentGame, SavedGameFilename + "?tag=" + CurrentGameID);

		if (CompanyManager.Instance != null) {
			CompanyManager.Instance.Save(CurrentGameID);
		}

		if (GameManager.Instance != null) {
			GameManager.Instance.Save(CurrentGameID);
		}

		if (WrestlerManager.Instance != null) {
			WrestlerManager.Instance.Save (CurrentGameID);
		}

		if (VenueManager.Instance != null) {
			VenueManager.Instance.Save (CurrentGameID);
		}
	}

	public void DeleteSaved(string gameID) {
		SavedGame toDelete = GetSavedGame(gameID);
		if (toDelete != null) {
			DeleteSaved (toDelete);
		}
		else {
			throw new UnityException(string.Format ("Unable to delete saved game {0}: No such game was found", gameID));
		}
	}

	public void DeleteSaved(SavedGame game) {
		GameManager.DeleteSaved(game.gameID);
		CompanyManager.DeleteSaved(game.gameID);
		WrestlerManager.DeleteSaved(game.gameID);
		VenueManager.DeleteSaved(game.gameID);

		games.Remove(game);
	}

	public void DeleteAllSaved() {
		while (GetSavedGames().Count > 0) {
			SavedGame game = GetSavedGames()[0];
			DeleteSaved (game);
		}

		if (ES2.Exists(SavedGameFilename)) {
			ES2.Delete (SavedGameFilename);
		}
	}

	public void Load(string gameID) {
		SavedGame toLoad = GetSavedGame(gameID);
		if (toLoad != null) {
			Load (toLoad);
		}
		else {
			throw new UnityException(string.Format ("Unable to load saved game {0}: No such game was found", gameID));
		}
	}
	
	public void Load(SavedGame game) {
		if (VenueManager.Instance != null) {
			VenueManager.Instance.Load (game.gameID);
		}

		if (WrestlerManager.Instance != null) {
			WrestlerManager.Instance.Load (game.gameID);
		}

		if (CompanyManager.Instance != null) {
			CompanyManager.Instance.Load(game.gameID);
		}

		if (GameManager.Instance != null) {
			GameManager.Instance.Load(game.gameID);
		}

		currentGame = game;
	}
}