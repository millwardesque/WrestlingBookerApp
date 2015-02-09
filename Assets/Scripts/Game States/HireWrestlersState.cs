using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class HireWrestlersState : GameState {
	GameManager gameManager;
	SelectOptionDialog wrestlerDialog;
	InfoDialog addAnotherDialog;
	List<Wrestler> wrestlers;

	public override void OnEnter (GameManager gameManager) {
		this.gameManager = gameManager;
		HireWrestler();
	}

	void HireWrestler() {
		wrestlerDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog();
		bool hasTwoPlusWrestlers = (gameManager.GetPlayerCompany().roster.Count > 1); // If the player doesn't have enough wrestlers, we won't let the player leave the hiring screen.

		wrestlerDialog.Initialize("Hire a wrestler", GetWrestlersForHire(), new UnityAction(OnHireWrestler), hasTwoPlusWrestlers, new UnityAction(DoneHiring));
	}

	void OnHireWrestler() {
		Wrestler hiredWrestler = wrestlers.Find ( x => x.wrestlerName == wrestlerDialog.GetSelectedOption().name );
		gameManager.GetPlayerCompany().roster.Add(hiredWrestler);

		bool hasTwoPlusWrestlers = (gameManager.GetPlayerCompany().roster.Count > 1); // If the player doesn't have enough wrestlers, we won't let the player leave the hiring screen.

		addAnotherDialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		addAnotherDialog.Initialize("Wrestler hired!", "You hired " + hiredWrestler.wrestlerName + "!\n" + (hasTwoPlusWrestlers ? "Would you like to hire another wrestler?" : "Now, choose another wrestler!"), new UnityAction(HireWrestler), hasTwoPlusWrestlers, new UnityAction(DoneHiring));
	}

	void DoneHiring() {
		gameManager.SetState(gameManager.FindState("IdleGameState"));
	}

	List<SelectOptionDialogOption> GetWrestlersForHire() {
		List<SelectOptionDialogOption> wrestlerOptions = new List<SelectOptionDialogOption>();
		wrestlers = gameManager.GetWrestlerManager().GetWrestlers();

		Company company = gameManager.GetPlayerCompany();
		foreach (Wrestler wrestler in wrestlers) {
			// If the wrestler isn't in the company roster, list as hireable.
			if (null == company.roster.Find( x => x.wrestlerName == wrestler.wrestlerName)) {
				wrestlerOptions.Add(new SelectOptionDialogOption(wrestler.wrestlerName, "Cost: " + wrestler.perMatchCost + "\n" + wrestler.description));
			}
		}
		
		return wrestlerOptions;
	}
}
