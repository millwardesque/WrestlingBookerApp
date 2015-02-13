using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ChooseMatchesGameState : GameState {
	GameManager gameManager;
	SinglesMatchDialog wrestlersDialog;
	SelectOptionDialog matchTypeDialog;
	SelectOptionDialog matchFinishDialog;
	SelectOptionDialog matchWinnerDialog;

	List<Wrestler> wrestlers;
	List<WrestlingMatchType> matchTypes;
	List<WrestlingMatchFinish> matchFinishes;

	public override void OnEnter (GameManager gameManager) {
		this.gameManager = gameManager;
		MakeNewMatch();
	}

	void OnWrestlersPicked() {
		matchTypeDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog(true);
		matchTypeDialog.Initialize("Match type", GetAvailableMatchTypes(), new UnityAction(OnMatchTypePicked));
	}

	void OnMatchTypePicked() {
		matchWinnerDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog(true);
		matchWinnerDialog.Initialize("Match winner", GetMatchWrestlers(), new UnityAction(OnMatchWinnerPicked));
	}

	void OnMatchWinnerPicked() {
		matchFinishDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog(true);
		matchFinishDialog.Initialize("Match finish", GetAvailableMatchFinishes(), new UnityAction(OnMatchFinishPicked));
	}

	void OnMatchFinishPicked() {
		OnMatchPicked ();
	}

	void OnMatchPicked() {
		WrestlingMatch match = new WrestlingMatch();
		Wrestler wrestler1 = wrestlers.Find( x => x.wrestlerName == wrestlersDialog.GetWrestler1().name );
		Wrestler wrestler2 = wrestlers.Find( x => x.wrestlerName == wrestlersDialog.GetWrestler2().name );
		match.teams.Add (new WrestlingTeam(wrestler1));
		match.teams.Add (new WrestlingTeam(wrestler2));
		match.type = matchTypes.Find ( x => x.typeName == matchTypeDialog.GetSelectedOption().name );
		match.finish = matchFinishes.Find ( x => x.finishName == matchFinishDialog.GetSelectedOption().name );

		WrestlingEvent currentEvent = gameManager.GetCurrentEvent();
		currentEvent.matches.Add(match);

		gameManager.OnWrestlingEventUpdated();

		InfoDialog makeAnotherDialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		makeAnotherDialog.Initialize("Add another match?", "Would you like to add another match?", new UnityAction(MakeNewMatch), true, new UnityAction(DoneWithMatches), "Yes", "No");
	}

	void MakeNewMatch() {
		wrestlersDialog = gameManager.GetGUIManager().InstantiateSinglesMatchDialog();
		wrestlersDialog.Initialize("Wrestlers", GetAvailableWrestlers(), GetAvailableWrestlers(), new UnityAction(OnWrestlersPicked));
	}

	void DoneWithMatches() {
		gameManager.SetState(gameManager.FindState("SellTicketsState"));
	}

	List<SelectOptionDialogOption> GetAvailableWrestlers() {
		List<SelectOptionDialogOption> wrestlerOptions = new List<SelectOptionDialogOption>();
		wrestlers = gameManager.GetPlayerCompany().roster;
		
		foreach (Wrestler wrestler in wrestlers) {
			wrestlerOptions.Add(new SelectOptionDialogOption(wrestler.wrestlerName, wrestler.description));
		}
		
		return wrestlerOptions;
	}

	List<SelectOptionDialogOption> GetAvailableMatchTypes() {
		List<SelectOptionDialogOption> matchTypeOptions = new List<SelectOptionDialogOption>();
		matchTypes = gameManager.GetMatchTypeManager().GetMatchTypes();
		
		foreach (WrestlingMatchType matchType in matchTypes) {
			matchTypeOptions.Add(new SelectOptionDialogOption(matchType.typeName, matchType.description));
		}
		
		return matchTypeOptions;
	}

	List<SelectOptionDialogOption> GetAvailableMatchFinishes() {
		List<SelectOptionDialogOption> matchFinishOptions = new List<SelectOptionDialogOption>();
		matchFinishes = gameManager.GetMatchFinishManager().GetMatchFinishes();
		
		foreach (WrestlingMatchFinish matchFinish in matchFinishes) {
			matchFinishOptions.Add(new SelectOptionDialogOption(matchFinish.finishName, matchFinish.description));
		}
		
		return matchFinishOptions;
	}

	List<SelectOptionDialogOption> GetMatchWrestlers() {
		List<SelectOptionDialogOption> matchWrestlers = new List<SelectOptionDialogOption>();
		matchWrestlers.Add (wrestlersDialog.GetWrestler1());
		matchWrestlers.Add (wrestlersDialog.GetWrestler2());

		return matchWrestlers;
	}
}
