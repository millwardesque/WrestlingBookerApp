using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ChooseEventTypeGameState : GameState {
	SelectOptionDialog eventTypeDialog;
	GameManager gameManager;
	List<EventType> types;
	
	public override void OnEnter(GameManager gameManager) {
		this.gameManager = gameManager;
		eventTypeDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog();
		eventTypeDialog.Initialize("Choose the event type", GetAvailableEventTypes(), new UnityAction(OnTypeSelected));
	}
	
	void OnTypeSelected() {
		EventType selected = types.Find( x => x.typeName == eventTypeDialog.GetSelectedOption().name );

		gameManager.GetCurrentEvent().Type = selected;
		gameManager.OnWrestlingEventUpdated();
		gameManager.SetState(gameManager.FindState("ChooseVenueGameState"));
	}

	List<SelectOptionDialogOption> GetAvailableEventTypes() {
		List<SelectOptionDialogOption> typeOptions = new List<SelectOptionDialogOption>();
		types = gameManager.GetEventTypeManager().GetTypes ();

		foreach (EventType type in types) {
			bool isInteractable = (type.cost <= gameManager.GetPlayerCompany().money);
			typeOptions.Add(new SelectOptionDialogOption(type.typeName, type.description, isInteractable));
		}

		return typeOptions;
	}
}
