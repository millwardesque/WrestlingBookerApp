using UnityEngine;
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
		}
		else {
			eventNameDialog = gameManager.GetGUIManager().InstantiateTextInputDialog();
			eventNameDialog.Initialize("Name your event", "", "Enter the name of your upcoming event.", new UnityAction(OnNameEntered));
		}
	}
	
	void OnNameEntered() {
		SetEventName(eventNameDialog.GetUserText());
	}

	void SetEventName(string name) {
		gameManager.GetCurrentEvent().eventName = name;
		gameManager.OnWrestlingEventUpdated();
		gameManager.SetState(gameManager.FindState("ChooseVenueGameState"));
	}
}
