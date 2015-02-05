using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class NameCompanyGameState : GameState {
	TextInputDialog companyNameDialog;
	GameManager gameManager;

	public override void OnEnter(GameManager gameManager) {
		this.gameManager = gameManager;
		companyNameDialog = gameManager.GetGUIManager().InstantiateTextInputDialog();
		companyNameDialog.Initialize("Name your company", "Enter the name of your new wrestling company.", new UnityAction(OnNameEntered));
	}

	void OnNameEntered() {
		gameManager.GetPlayerCompany().companyName = companyNameDialog.GetUserText();
		gameManager.OnCompanyUpdated();
	
		gameManager.GetGUIManager().ShowStatusPanel();
		gameManager.SetState(gameManager.GetDelayedGameState(gameManager.FindState("IdleGameState")));
	}
}
