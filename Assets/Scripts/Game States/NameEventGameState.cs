using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class NameEventGameState : GameState {
	TextInputDialog eventNameDialog;
	GameManager gameManager;
	
	public override void OnEnter(GameManager gameManager) {
		this.gameManager = gameManager;
		eventNameDialog = gameManager.GetGUIManager().InstantiateTextInputDialog();
		int eventCount = gameManager.GetPlayerCompany().eventHistory.Count;
		eventNameDialog.Initialize("Name your event", "Event #" + (eventCount + 1), "Enter the name of your upcoming event.", new UnityAction(OnNameEntered));
	}
	
	void OnNameEntered() {
		gameManager.GetCurrentEvent().eventName = eventNameDialog.GetUserText();
		gameManager.OnWrestlingEventUpdated();
		gameManager.SetState(gameManager.FindState("ChooseEventTypeGameState"));
	}
}
