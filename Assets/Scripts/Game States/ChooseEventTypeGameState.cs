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

		List<SelectOptionDialogOption> availableEventTypes = GetAvailableEventTypes();
		if (availableEventTypes.Count > 0) {
			eventTypeDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog(true);
			eventTypeDialog.Initialize("Choose the event type", availableEventTypes, new UnityAction(OnTypeSelected));
		}
		else {
			InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
			dialog.Initialize("No events available", "You're all out of money!\nTry loaning out some wrestlers to another organization to make some more cash.", new UnityAction(OnNoneAvailableAcknowledge));
		}
	}
	
	void OnTypeSelected() {
		EventType selected = types.Find( x => x.typeName == eventTypeDialog.GetSelectedOption().name );

		gameManager.GetCurrentEvent().Type = selected;
		gameManager.OnWrestlingEventUpdated();
		gameManager.SetState(gameManager.FindState("NameEventGameState"));
	}

 	void OnNoneAvailableAcknowledge() {
		gameManager.SetState(gameManager.FindState("IdleGameState"));
	}

	List<SelectOptionDialogOption> GetAvailableEventTypes() {
		List<SelectOptionDialogOption> typeOptions = new List<SelectOptionDialogOption>();
		types = gameManager.GetEventTypeManager().GetTypes ();

		foreach (EventType type in types) {
			bool isInteractable = (type.cost <= gameManager.GetPlayerCompany().money);

			if (isInteractable) {
				typeOptions.Add(new SelectOptionDialogOption(type.typeName, type.description, isInteractable));
			}
		}

		return typeOptions;
	}
}
