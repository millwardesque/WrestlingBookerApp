﻿	using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class NameEventGameState : GameState {
	TextInputDialog eventNameDialog;
	GameManager gameManager;
	
	public override void OnEnter(GameManager gameManager) {
		this.gameManager = gameManager;
		int eventNumber = gameManager.GetPlayerCompany().eventHistory.Count + 1;

		if (gameManager.GetCurrentEvent().Type.typeName == "House show") { // Bypass selection for house shows and move right on to the next step.
			SetEventName("Event #" + eventNumber);
			return;
		}

		// Default to the last name used for a TV since TV shows don't change titles.
		string defaultName = "";
		if (gameManager.GetCurrentEvent ().Type.typeName == "TV") {
			foreach (HistoricalWrestlingEvent wrestlingEvent in gameManager.GetPlayerCompany().eventHistory) {
				if (wrestlingEvent.type == "TV") {
					defaultName = wrestlingEvent.name;
				}
			}
		}

		eventNameDialog = gameManager.GetGUIManager().InstantiateTextInputDialog();
		eventNameDialog.Initialize("Name your event", defaultName, "Enter the name of your upcoming event.", new UnityAction(OnNameEntered));
	}
	
	void OnNameEntered() {
		SetEventName(eventNameDialog.GetUserText());
	}

	void SetEventName(string name) {
		gameManager.GetCurrentEvent().eventName = name;
		gameManager.OnWrestlingEventUpdated();
		ExecuteTransition("FINISHED");
	}
}
