﻿using UnityEngine;
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
			eventTypeDialog.Initialize("Event type", availableEventTypes, new UnityAction(OnTypeSelected), true, new UnityAction(OnCancel));
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
		ExecuteTransition("EventTypeChosen");
	}

 	void OnNoneAvailableAcknowledge() {
		ExecuteTransition("NoEventsAvailable");
	}

	List<SelectOptionDialogOption> GetAvailableEventTypes() {
		List<SelectOptionDialogOption> typeOptions = new List<SelectOptionDialogOption>();
		types = gameManager.GetEventTypeManager().GetTypes (gameManager.GetPhase());

		foreach (EventType type in types) {
			bool isInteractable = (type.cost <= gameManager.GetPlayerCompany().money);

			if (isInteractable) {
				typeOptions.Add(new SelectOptionDialogOption(type.typeName, string.Format ("${0}", type.cost), string.Format ("Cost: ${0}\n{1}", type.cost, type.description), isInteractable));
			}
		}

		return typeOptions;
	}

	void OnCancel() {
		ExecuteTransition("Cancel");
	}
}
