using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class RunEventState : GameState {
	List<WrestlingMatch> matches;
	WrestlingEvent currentEvent;
	InfoDialog matchDialog;
	GameManager gameManager;
	int currentMatchIndex = 0;

	public override void OnEnter (GameManager gameManager) {
		this.gameManager = gameManager;
		matches = gameManager.GetCurrentEvent().matches;
		currentEvent = gameManager.GetCurrentEvent();

		if (matches.Count > 0) {
			ProcessNextMatch ();
		}
		else {
			Debug.LogError("Unable to run event: There are no matches set.");
			FinishedRunningEvent();
		}
	}

	void ProcessNextMatch() {
		WrestlingMatch match = matches[currentMatchIndex];
		currentMatchIndex++;
		bool unlockedMatchType = false;

		match.rating = currentEvent.EventVenue.GetMatchTypePreference(match.type) + currentEvent.EventVenue.GetMatchFinishPreference(match.finish);
		match.rating /= 2.0f;

		if (currentEvent.EventVenue.GetMatchTypePreference(match.type) > 0.5 && !unlockedMatchType) {
		    gameManager.GetPlayerCompany().AttemptUnlockMatchTypeByVenue(currentEvent.EventVenue);
			unlockedMatchType = true;
		}

		string matchReport = "";
		matchReport += match.VersusString() + "\n";
		matchReport += string.Format("The fans thought the match type was a {0}/10\n", Mathf.RoundToInt(currentEvent.EventVenue.GetMatchTypePreference(match.type) * 10.0f));
		matchReport += string.Format("They thought the finish was a {0}/10\n", Mathf.RoundToInt(currentEvent.EventVenue.GetMatchFinishPreference(match.finish) * 10.0f));
		matchReport += string.Format("Overall, they rated the match {0}/10", Mathf.RoundToInt(match.rating * 10.0f));

		// Note: currentMatchIndex is used as-is in the dialog title because it's already been incremented, eliminating the need to add one to eliminate zero-indexing confusion.
		matchDialog = gameManager.GetGUIManager ().InstantiateInfoDialog();
		matchDialog.Initialize("Match #" + currentMatchIndex, matchReport, (currentMatchIndex < matches.Count ? new UnityAction(ProcessNextMatch) : new UnityAction(FinishedRunningEvent)));
	}

	void FinishedRunningEvent() {
		gameManager.SetState(gameManager.FindState("EventFinishedState"));
	}
}
