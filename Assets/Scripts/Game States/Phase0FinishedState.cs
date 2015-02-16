using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Phase0FinishedState : GameState {
	public override void OnEnter(GameManager gameManager) {		
		string message = string.Format ("Congratulations, it looks like you've got the hang of things. Your roster size has increased to {0}, so hire some more wrestlers and start holding bigger and better events!", gameManager.GetPlayerCompany().maxRosterSize);
		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		dialog.Initialize("Moving on up!", message, new UnityAction(OnFinished));
	}
}
