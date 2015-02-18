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
		bool hasMoreWrestlersToHire = (GetWrestlersForHire().Count > 0);

		if (!gameManager.GetPlayerCompany().CanAddWrestlers()) {
			InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
			dialog.Initialize("Hire a wrestler", "You don't have any more space in your roster.", new UnityAction(DoneHiring));
		}
		else if (hasMoreWrestlersToHire) {
			bool hasTwoPlusWrestlers = (gameManager.GetPlayerCompany().GetRoster ().Count > 1); // If the player doesn't have enough wrestlers, we won't let the player leave the hiring screen.
			wrestlerDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog(true);
			wrestlerDialog.Initialize("Hire a wrestler", GetWrestlersForHire(), new UnityAction(OnHireWrestler), hasTwoPlusWrestlers, new UnityAction(DoneHiring));
		}
		else {
			InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
			dialog.Initialize("Hire a wrestler", "There aren't any more wrestlers available for hire. Please check back later!", new UnityAction(DoneHiring));
		}
	}

	void OnHireWrestler() {
		Wrestler hiredWrestler = wrestlers.Find ( x => x.wrestlerName == wrestlerDialog.GetSelectedOption().name );
		gameManager.GetPlayerCompany().AddWrestlerToRoster(hiredWrestler);
		gameManager.GetPlayerCompany().money -= hiredWrestler.hiringCost;
		gameManager.OnCompanyUpdated();

		bool hasTwoPlusWrestlers = (gameManager.GetPlayerCompany().GetRoster().Count > 1); // If the player doesn't have enough wrestlers, we won't let the player leave the hiring screen.
		bool hasMoreWrestlersToHire = (GetWrestlersForHire().Count > 0);

		addAnotherDialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		if (hasMoreWrestlersToHire) {
			if (gameManager.GetPlayerCompany().CanAddWrestlers()) {
				addAnotherDialog.Initialize("Wrestler hired!", "You hired " + hiredWrestler.wrestlerName + "!\n" + (hasTwoPlusWrestlers ? "Would you like to hire another wrestler?" : "Now, choose another wrestler!"), new UnityAction(HireWrestler), hasTwoPlusWrestlers, new UnityAction(DoneHiring), "Yes", "No");
			}
			else {
				addAnotherDialog.Initialize("Wrestler hired!", "You hired " + hiredWrestler.wrestlerName + "!\nYour roster is now full.", new UnityAction(DoneHiring));
			}
		}
		else {
			addAnotherDialog.Initialize("Wrestler hired!", "You hired " + hiredWrestler.wrestlerName + "!\nThere aren't any more wrestlers available for hire.", new UnityAction(DoneHiring));
		}
	}

	void DoneHiring() {
		ExecuteTransition("FINISHED");
	}

	List<SelectOptionDialogOption> GetWrestlersForHire() {
		List<SelectOptionDialogOption> wrestlerOptions = new List<SelectOptionDialogOption>();
		wrestlers = gameManager.GetWrestlerManager().GetWrestlers(gameManager.GetPhase());

		Company company = gameManager.GetPlayerCompany();
		foreach (Wrestler wrestler in wrestlers) {
			// If the wrestler isn't in the company roster, list as hireable.
			if (null == company.GetRoster ().Find( x => x.wrestlerName == wrestler.wrestlerName) && company.money >= wrestler.hiringCost) {
				wrestlerOptions.Add(new SelectOptionDialogOption(wrestler.wrestlerName, "Hire: $" + wrestler.hiringCost + "\n" + wrestler.DescriptionWithStats));
			}
		}
		
		return wrestlerOptions;
	}
}
