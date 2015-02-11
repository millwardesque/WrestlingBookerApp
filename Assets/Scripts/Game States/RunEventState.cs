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

		match.rating = currentEvent.EventVenue.GetMatchTypePreference(match.type) + currentEvent.EventVenue.GetMatchFinishPreference(match.finish);
		match.rating /= 2.0f;

		// Note: currentMatchIndex is used as-is in the dialog title because it's already been incremented, eliminating the need to add one to eliminate zero-indexing confusion.
		matchDialog = gameManager.GetGUIManager ().InstantiateInfoDialog();
		matchDialog.Initialize("Match #" + currentMatchIndex, string.Format ("{0}\nRated {1} / 10", match.VersusString(), Mathf.RoundToInt(match.rating * 10.0f)), (currentMatchIndex < matches.Count ? new UnityAction(ProcessNextMatch) : new UnityAction(FinishedRunningEvent)));
	}

	void FinishedRunningEvent() {
		gameManager.SetState(gameManager.FindState("EventFinishedState"));
	}
}
