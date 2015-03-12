using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ChooseVenueGameState : GameState {
	PagedSelectOptionDialog venueDialog;
	GameManager gameManager;
	List<Venue> venues;
	
	public override void OnEnter(GameManager gameManager) {
		this.gameManager = gameManager;
		venueDialog = gameManager.GetGUIManager().InstantiatePagedSelectOptionDialog();
		venueDialog.Initialize("Choose the venue", GetAvailableVenues(), new UnityAction(OnVenueSelected));
	}
	
	void OnVenueSelected() {
		Venue selected = venues.Find( x => x.venueName == venueDialog.GetSelectedOption().name );

		gameManager.GetCurrentEvent().EventVenue = selected;
		gameManager.OnWrestlingEventUpdated();
		ExecuteTransition("FINISHED");
	}
	
	List<SelectOptionDialogOption> GetAvailableVenues() {
		venues = gameManager.GetPlayerCompany().unlockedVenues;
		List<SelectOptionDialogOption> venueOptions = new List<SelectOptionDialogOption>();

		foreach (Venue venue in venues) {
			bool isInteractable = (venue.baseCost <= gameManager.GetPlayerCompany().money);
			string description = string.Format ("Cost: ${0} upfront + {1}% of the gate\nCapacity: {2}\nWrestling Popularity: {3}\n\n{4}",
			                                    venue.baseCost, Mathf.RoundToInt(venue.gatePercentage * 100.0f), venue.capacity, Utilities.AlphaRating(venue.popularity), venue.venueDescription);
			venueOptions.Add(new SelectOptionDialogOption(venue.venueName, Utilities.AlphaRating(venue.popularity), description, isInteractable));
		}
		
		return venueOptions;
	}
}
