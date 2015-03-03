using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class FireWrestlerState : GameState {
	GameManager gameManager;
	List<Wrestler> wrestlers;
	SelectOptionDialog wrestlerDialog;
	string cantFireMessage = "You can't fire any more wrestlers because you need at least two in your roster at all times.";

	public override void OnEnter (GameManager gameManager) {
		this.gameManager = gameManager;
		FireWrestler();
	}
	
	void FireWrestler() {
		bool hasMoreWrestlersToFire = (GetWrestlers().Count > 2);
		
		if (hasMoreWrestlersToFire) {
			wrestlerDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog(true);
			wrestlerDialog.Initialize("Fire a wrestler", GetWrestlers(), new UnityAction(OnFireWrestler), true, new UnityAction(DoneFiring));
		}
		else {
			InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
			dialog.Initialize("Fire a wrestler", cantFireMessage, new UnityAction(DoneFiring));
		}
	}
	
	void OnFireWrestler() {
		Wrestler firedWrestler = wrestlers.Find ( x => x.wrestlerName == wrestlerDialog.GetSelectedOption().name );
		gameManager.GetPlayerCompany().RemoveFromRoster(firedWrestler);
		gameManager.OnCompanyUpdated();
		
		bool canFireMoreWrestlers = (gameManager.GetPlayerCompany().GetRoster().Count > 2);
		
		InfoDialog addAnotherDialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		if (canFireMoreWrestlers) {
			addAnotherDialog.Initialize("Wrestler fired!", "You fired " + firedWrestler.wrestlerName + "!\n" + "Would you like to fire another wrestler?", new UnityAction(FireWrestler), true, new UnityAction(DoneFiring), "Yes", "No");
		}
		else {
			addAnotherDialog.Initialize("Wrestler fired!", "You fired " + firedWrestler.wrestlerName + "!\n" + cantFireMessage, new UnityAction(DoneFiring));
		}
	}
	
	void DoneFiring() {
		ExecuteTransition("FINISHED");
	}
	
	List<SelectOptionDialogOption> GetWrestlers() {
		List<SelectOptionDialogOption> wrestlerOptions = new List<SelectOptionDialogOption>();
		wrestlers = gameManager.GetPlayerCompany().GetRoster();

		foreach (Wrestler wrestler in wrestlers) {
			wrestlerOptions.Add(new SelectOptionDialogOption(wrestler.wrestlerName, "", wrestler.DescriptionWithStats));
		}
		
		return wrestlerOptions;
	}
}
