using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ChooseMatchesGameState : GameState {
	GameManager gameManager;
	SinglesMatchDialog dialog;
	List<Wrestler> wrestlers;

	public override void OnEnter (GameManager gameManager) {
		this.gameManager = gameManager;
		dialog = gameManager.GetGUIManager().InstantiateSinglesMatchDialog();
		dialog.Initialize("Choose the main event wrestlers", GetAvailableWrestlers(), GetAvailableWrestlers(), new UnityAction(OnMatchPicked));
	}

	void OnMatchPicked() {
		// @TODO Pull match from dialog box.

		gameManager.OnWrestlingEventUpdated();
		gameManager.SetState(gameManager.FindState("SellTicketsState"));
	}

	List<SelectOptionDialogOption> GetAvailableWrestlers() {
		List<SelectOptionDialogOption> wrestlerOptions = new List<SelectOptionDialogOption>();
		wrestlers = gameManager.GetWrestlerManager().GetWrestlers();
		
		foreach (Wrestler wrestler in wrestlers) {
			wrestlerOptions.Add(new SelectOptionDialogOption(wrestler.wrestlerName, wrestler.description));
		}
		
		return wrestlerOptions;
	}
}
