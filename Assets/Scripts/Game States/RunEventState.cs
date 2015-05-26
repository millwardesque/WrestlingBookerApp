using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class RunEventState : GameState {
	List<WrestlingMatch> matches;
	WrestlingEvent currentEvent;
	InfoSliderDialog matchDialog;
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
		float matchFinishWeight = 0.4f;
		float matchWrestlerPerformanceWeight = 0.5f;

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
		matchReport += GetFanMatchTypeReview(matchTypeRating) + "\n";
		matchReport += GetFanPerformanceReview(wrestlerPerformanceRating) + "\n";
		matchReport += GetFanFinishReview(matchFinishRating) + "\n";
		matchReport += "\n" + GetFanMatchReview(match.rating) + "\n";

		// Note: currentMatchIndex is used as-is in the dialog title because it's already been incremented, eliminating the need to add one to eliminate zero-indexing confusion.
		matchDialog = gameManager.GetGUIManager ().InstantiateInfoSliderDialog();
		matchDialog.Initialize(match.VersusString(), matchReport, "", 0f, 1f, match.rating, 1f, (currentMatchIndex < matches.Count ? new UnityAction(ProcessNextMatch) : new UnityAction(FinishedRunningEvent)));
	}

	void FinishedRunningEvent() {
		ExecuteTransition("FINISHED");
	}

	string GetFanMatchTypeReview(float rating) {
		string review = "";

		if (rating > 0.9) {
			review = "They love this type of match.";
		}
		else if (rating > 0.7) {
			review = "This type of match was well-received by the crowd.";
		}
		else if (rating > 0.5) {
			review = "They seemed fine with the type of match.";
		}
		else if (rating > 0.3) {
			review = "The crowd didn't seem to interested by the type of match.";
		}
		else {
			review = "They didn't care for this type of match at all.";
		}

		return review;
	}

	string GetFanFinishReview(float rating) {
		string review = "";
		
		if (rating > 0.9) {
			review = "The finish for this match seemed to really ignite the crowd.";
		}
		else if (rating > 0.7) {
			review = "The crowd dug the match finish.";
		}
		else if (rating > 0.5) {
			review = "The response to the finish was alright.";
		}
		else if (rating > 0.3) {
			review = "The finish didn't really a get a reaction.";
		}
		else {
			review = "They seemed to hate the finish.";
		}
		
		return review;
	}

	string GetFanPerformanceReview(float rating) {
		string review = "";
		if (rating > 0.9) {
			review = "The audience really loved the performance the wrestlers put on.";
		}
		else if (rating > 0.7) {
			review = "There were some definite edge-of-the-seat moments here.";
		}
		else if (rating > 0.5) {
			review = "The audience were interested for most of the match.";
		}
		else if (rating > 0.3) {
			review = "The match had a couple of good spots, but was mostly forgettable.";
		}
		else {
			review = "The audience seemed pretty unimpressed with the wrestlers' performance.";
		}
		
		return review;
	}

	string GetFanMatchReview(float rating) {
		string review = "";
		if (rating >= 0.9) {
			review = "This is a match-of-the-year candidate!.";
		}
		else if (rating > 0.7) {
			review = "This was a great match, the crowd loved it.";
		}
		else if (rating > 0.5) {
			review = "The crowd seems content with this match.";
		}
		else if (rating > 0.3) {
			review = "There were some definite flaws, but the crowd liked some of it.";
		}
		else {
			review = "They hated this match. A real stinker.";
		}
		
		return review;
	}
}
