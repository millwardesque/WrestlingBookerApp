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
		matchReport += GetFanPerformanceReview(wrestlerPerformanceRating) + "\n";
		matchReport += GetFanMatchTypeReview(matchTypeRating) + "\n";
		matchReport += GetFanFinishReview(matchFinishRating) + "\n";
		matchReport += GetFanMatchReview(match.rating) + "\n";

		// Note: currentMatchIndex is used as-is in the dialog title because it's already been incremented, eliminating the need to add one to eliminate zero-indexing confusion.
		matchDialog = gameManager.GetGUIManager ().InstantiateInfoDialog();
		matchDialog.Initialize("Match #" + currentMatchIndex, matchReport, (currentMatchIndex < matches.Count ? new UnityAction(ProcessNextMatch) : new UnityAction(FinishedRunningEvent)));
	}

	void FinishedRunningEvent() {
		gameManager.ReplaceState(gameManager.FindState("EventFinishedState"));
	}

	string GetFanMatchTypeReview(float rating) {
		string review = "";

		if (rating > 0.7) {
			review = "They really like this type of match.";
		}
		else if (rating > 0.5) {
			review = "They seemed fine with the type of match.";
		}
		else {
			review = "They didn't care for this type of match at all.";
		}

		return review;
	}

	string GetFanFinishReview(float rating) {
		string review = "";
		
		if (rating > 0.7) {
			review = "The finish for this match seemed to really ignite the crowd.";
		}
		else if (rating > 0.5) {
			review = "The response to the finish was alright.";
		}
		else {
			review = "They seemed to hate the finish.";
		}
		
		return review;
	}

	string GetFanPerformanceReview(float rating) {
		string review = "";
		if (rating > 0.7) {
			review = "The audience really loved the performance the wrestlers put on.";
		}
		else if (rating > 0.5) {
			review = "The audience were interested for most of the match.";
		}
		else {
			review = "The audience seemed pretty unimpressed with the wrestlers' performance.";
		}
		
		return review;
	}

	string GetFanMatchReview(float rating) {
		string review = "";
		if (rating >= 0.9) {
			review = "In their eyes, this is a match-of-the-year candidate!.";
		}
		else if (rating > 0.7) {
			review = "This was a great match, they definitely enjoyed it.";
		}
		else if (rating > 0.5) {
			review = "They seem content with this match.";
		}
		else {
			review = "They hated this match. A real stinker.";
		}
		
		return review;
	}
}
