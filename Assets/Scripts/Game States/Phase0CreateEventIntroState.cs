using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Phase0CreateEventIntroState : GameState {
	public override void OnEnter(GameManager gameManager) {		
		string message = "Now that you've got some wrestlers, lets put them to work in our first event.\n\nFirst, click the 'Create Event' button on the main screen.";
		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		dialog.Initialize("Create your first event!", message, new UnityAction(OnIntroFinished));
	}
	
	void OnIntroFinished() {
		ExecuteTransition("FINISHED");
	}
}
