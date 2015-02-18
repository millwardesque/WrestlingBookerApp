﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ChooseMatchesGameState : GameState {
	GameManager gameManager;
	WrestlingMatch match = new WrestlingMatch();
	SinglesMatchDialog wrestlersDialog;
	SelectOptionDialog matchTypeDialog;
	SelectOptionDialog matchFinishDialog;
	SelectOptionDialog matchWinnerDialog;

	List<Wrestler> usedWrestlers = new List<Wrestler>();
	List<Wrestler> wrestlers;
	List<WrestlingMatchType> matchTypes;
	List<WrestlingMatchFinish> matchFinishes;

	public override void OnEnter (GameManager gameManager) {
		this.gameManager = gameManager;
		MakeNewMatch();
	}

	void OnWrestlersPicked() {
		Wrestler wrestler1 = wrestlers.Find( x => x.wrestlerName == wrestlersDialog.GetWrestler1().name );
		Wrestler wrestler2 = wrestlers.Find( x => x.wrestlerName == wrestlersDialog.GetWrestler2().name );
		match.teams.Add (new WrestlingTeam(wrestler1));
		match.teams.Add (new WrestlingTeam(wrestler2));
		usedWrestlers.Add (wrestler1);
		usedWrestlers.Add (wrestler2);

		matchTypeDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog(true);
		matchTypeDialog.Initialize("Match type", GetAvailableMatchTypes(), new UnityAction(OnMatchTypePicked));
	}

	void OnMatchTypePicked() {
		match.type = matchTypes.Find ( x => x.typeName == matchTypeDialog.GetSelectedOption().name );

		matchFinishDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog(true);
		matchFinishDialog.Initialize("Match finish", GetAvailableMatchFinishes(), new UnityAction(OnMatchFinishPicked));
	}

	void OnMatchFinishPicked() {
		match.finish = matchFinishes.Find ( x => x.finishName == matchFinishDialog.GetSelectedOption().name );

		OnMatchPicked ();
	}

	void OnMatchPicked() {
		WrestlingEvent currentEvent = gameManager.GetCurrentEvent();
		currentEvent.matches.Add(match);

		gameManager.OnWrestlingEventUpdated();

		InfoDialog makeAnotherDialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		if (wrestlers.Count - usedWrestlers.Count >= 2) {
			makeAnotherDialog.Initialize("Add another match?", "Would you like to add another match?", new UnityAction(MakeNewMatch), true, new UnityAction(DoneWithMatches), "Yes", "No");
		}
		else {
			makeAnotherDialog.Initialize("Card finished", "All your wrestlers have a spot on the card, great job!", new UnityAction(DoneWithMatches));
		}
	}

	void MakeNewMatch() {
		wrestlersDialog = gameManager.GetGUIManager().InstantiateSinglesMatchDialog();
		wrestlersDialog.Initialize("Wrestlers", GetAvailableWrestlers(), GetAvailableWrestlers(), new UnityAction(OnWrestlersPicked));
	}

	void DoneWithMatches() {
		gameManager.ReplaceState(gameManager.FindState("SellTicketsState"));
	}

	List<SelectOptionDialogOption> GetAvailableWrestlers() {
		List<SelectOptionDialogOption> wrestlerOptions = new List<SelectOptionDialogOption>();
		wrestlers = gameManager.GetPlayerCompany().GetRoster();
		
		foreach (Wrestler wrestler in wrestlers) {
			if (!usedWrestlers.Contains(wrestler)) {
				wrestlerOptions.Add(new SelectOptionDialogOption(wrestler.wrestlerName, wrestler.DescriptionWithStats));
			}
		}
		
		return wrestlerOptions;
	}

	List<SelectOptionDialogOption> GetAvailableMatchTypes() {
		List<SelectOptionDialogOption> matchTypeOptions = new List<SelectOptionDialogOption>();
		matchTypes = gameManager.GetPlayerCompany().unlockedMatchTypes;
		
		foreach (WrestlingMatchType matchType in matchTypes) {
			float localPreference = gameManager.GetCurrentEvent().EventVenue.GetMatchTypePreference(matchType);
			string localPreferenceString = Utilities.AlphaRating(localPreference);

			float wrestlerEffectiveness = 0;
			foreach (Wrestler wrestler in match.Participants) {
				wrestlerEffectiveness += wrestler.GetMatchTypeAffinity(matchType) / match.ParticipantCount;
			}
			string wrestlerEffectivenessString = Utilities.AlphaRating(wrestlerEffectiveness);

			string matchTypeDescription = string.Format("Local popularity: {0}\nWrestler effectiveness: {1}\n{2}", localPreferenceString, wrestlerEffectivenessString, matchType.description);
			matchTypeOptions.Add(new SelectOptionDialogOption(matchType.typeName, matchTypeDescription));
		}
		
		return matchTypeOptions;
	}

	List<SelectOptionDialogOption> GetAvailableMatchFinishes() {
		List<SelectOptionDialogOption> matchFinishOptions = new List<SelectOptionDialogOption>();
		matchFinishes = gameManager.GetMatchFinishManager().GetMatchFinishes(gameManager.GetPhase());
		
		foreach (WrestlingMatchFinish matchFinish in matchFinishes) {

			float localPreference = gameManager.GetCurrentEvent().EventVenue.GetMatchFinishPreference(matchFinish);
			string localPreferenceString = Utilities.AlphaRating(localPreference);

			string matchFinishDescription = string.Format("Local popularity: {0}\n{1}", localPreferenceString, matchFinish.description);
			matchFinishOptions.Add(new SelectOptionDialogOption(matchFinish.finishName, matchFinishDescription));
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
