using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ChooseVenueGameState : GameState {
	SelectOptionDialog venueDialog;
	GameManager gameManager;
	List<Venue> venues;
	
	public override void OnEnter(GameManager gameManager) {
		this.gameManager = gameManager;
		venueDialog = gameManager.GetGUIManager().InstantiateSelectOptionDialog(true);
		venueDialog.Initialize("Choose the venue", GetAvailableVenues(), new UnityAction(OnVenueSelected));
	}
	
	void OnVenueSelected() {
		Venue selected = venues.Find( x => x.venueName == venueDialog.GetSelectedOption().name );

		gameManager.GetCurrentEvent().EventVenue = selected;
		gameManager.OnWrestlingEventUpdated();
		gameManager.SetState(gameManager.FindState("ChooseMatchesGameState"));
	}
	
	List<SelectOptionDialogOption> GetAvailableVenues() {
		venues = gameManager.GetVenueManager().GetVenues();
		List<SelectOptionDialogOption> venueOptions = new List<SelectOptionDialogOption>();

		foreach (Venue venue in venues) {
			bool isInteractable = (venue.baseCost <= gameManager.GetPlayerCompany().money);
			venueOptions.Add(new SelectOptionDialogOption(venue.venueName, venue.venueDescription, isInteractable));
		}
		
		return venueOptions;
	}
}
