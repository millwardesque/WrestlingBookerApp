using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ChooseMatchesGameState : GameState {
	GameManager gameManager;

	public override void OnEnter (GameManager gameManager) {
		Debug.Log ("TODO: match selection");
		gameManager.SetState(gameManager.FindState("SellTicketsState"));
	}
}
