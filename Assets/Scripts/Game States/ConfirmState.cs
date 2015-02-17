using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ConfirmState : GameState {
	string title;
	string message;
	UnityAction okAction;
	UnityAction cancelAction;

	public void Initialize(string title, string message, UnityAction okAction, UnityAction cancelAction = null) {
		this.title = title;
		this.message = message;
		this.okAction = okAction;
		this.cancelAction = cancelAction;
	}

	public override void OnEnter(GameManager gameManager) {		
		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		dialog.Initialize(title, message, okAction, (cancelAction != null), cancelAction, "Yes", "No");
	}
}
