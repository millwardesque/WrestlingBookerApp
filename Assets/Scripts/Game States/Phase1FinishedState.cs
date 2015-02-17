using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Phase1FinishedState : GameState {
	public override void OnEnter(GameManager gameManager) {		
		string message = string.Format ("Wow, you've made some good progress! Your roster size has increased to {0}, and you can now run TV events to make even more money!", gameManager.GetPlayerCompany().maxRosterSize);
		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		dialog.Initialize("Moving on up!", message, new UnityAction(OnFinished));
	}
}
