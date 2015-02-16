using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Phase0IntroGameState : GameState {
	GameManager gameManager;
	
	public override void OnEnter(GameManager gameManager) {
		this.gameManager = gameManager;

		string message = string.Format("Congratulations on the creation of {0}! First thing's first, we need to get some talent!", gameManager.GetPlayerCompany().companyName);
		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		dialog.Initialize("Welcome!", message, new UnityAction(OnWelcomeFinished));
	}

	void OnWelcomeFinished() {
		string message = string.Format("You currently have the resources to support {0} wrestlers, so choose wisely.", gameManager.GetPlayerCompany().maxRosterSize);

		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		dialog.Initialize("Roster selection", message, new UnityAction(OnFinished));
	}
}
