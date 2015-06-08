using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour {
	public Button continueButton;
	List<SavedGame> savedGames;

	void Start() {
		savedGames = SavedGameManager.Instance.GetSavedGames();
		if (savedGames.Count > 0) {
			continueButton.interactable = true;
		}
		else {
			continueButton.interactable = false;
		}
	}

	public void StartNewGame() {
		SavedGameManager.Instance.ClearSavedGames();
		SavedGameManager.Instance.ClearCurrentGame();
		SavedGameManager.Instance.CreateNewGame();

		Application.LoadLevel("Sandbox");
	}

	public void ContinueGame() {
		if (savedGames.Count == 0) {
			Debug.LogError("Unable to continue game: There are no saved games, but the Continue button seems to be active.");
			return;
		}
		else {
			SavedGameManager.Instance.SetCurrentGame(savedGames[0]);
			Application.LoadLevel("Sandbox");
		}
	}
}
