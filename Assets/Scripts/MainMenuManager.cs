using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
	public Button continueButton;

	void Start() {
		if (PlayerPrefs.HasKey("playerCompany")) {
			continueButton.interactable = true;
		}
		else {
			continueButton.interactable = false;
		}

	}

	public void StartNewGame() {
		PlayerPrefs.DeleteKey("playerCompany");
		Application.LoadLevel("Sandbox");
	}

	public void ContinueGame() {
		Application.LoadLevel("Sandbox");
	}
}
