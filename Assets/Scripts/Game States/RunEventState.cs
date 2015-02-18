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

		// These weights should all add up to 1.0f
		float matchTypeWeight = 0.1f;
		float matchFinishWeight = 0.3f;
		float matchWrestlerPerformanceWeight = 0.6f;

		float matchTypeRating = currentEvent.EventVenue.GetMatchTypePreference(match.type);
		float matchFinishRating = currentEvent.EventVenue.GetMatchFinishPreference(match.finish);

		// Add wrestler affinities.
		float wrestlerPerformanceRating = 0.0f;
		if (match.ParticipantCount > 0) {
			foreach (Wrestler wrestler in match.Participants) {
				wrestlerPerformanceRating += wrestler.GetMatchTypeAffinity(match.type) * wrestler.work;
			}
			wrestlerPerformanceRating /= match.ParticipantCount;
		}

		match.rating = (matchTypeRating * matchTypeWeight) + (matchFinishRating * matchFinishWeight) + (wrestlerPerformanceRating * matchWrestlerPerformanceWeight);

		if (currentEvent.EventVenue.GetMatchTypePreference(match.type) > 0.5 && !unlockedMatchType) {
		    gameManager.GetPlayerCompany().AttemptUnlockMatchTypeByVenue(currentEvent.EventVenue);
			unlockedMatchType = true;
		}

		string matchReport = "";
		matchReport += match.VersusString() + "\n";
		matchReport += string.Format("Fans thought the wrestlers' performance was a {0}/10\n", Mathf.RoundToInt(wrestlerPerformanceRating * 10.0f));
		matchReport += string.Format("They thought the match type was a {0}/10\n", Mathf.RoundToInt(matchTypeRating * 10.0f));
		matchReport += string.Format("They thought the finish was a {0}/10\n", Mathf.RoundToInt(matchFinishRating * 10.0f));
		matchReport += string.Format("Overall, they rated the match {0}/10", Mathf.RoundToInt(match.rating * 10.0f));

		// Note: currentMatchIndex is used as-is in the dialog title because it's already been incremented, eliminating the need to add one to eliminate zero-indexing confusion.
		matchDialog = gameManager.GetGUIManager ().InstantiateInfoDialog();
		matchDialog.Initialize("Match #" + currentMatchIndex, matchReport, (currentMatchIndex < matches.Count ? new UnityAction(ProcessNextMatch) : new UnityAction(FinishedRunningEvent)));
	}

	void FinishedRunningEvent() {
		gameManager.ReplaceState(gameManager.FindState("EventFinishedState"));
	}
}
