using UnityEngine;
using System.Collections;

public class RunEventState : GameState {
	
	public override void OnEnter (GameManager gameManager) {
		Debug.Log ("TODO: Run event");
		gameManager.SetState(gameManager.FindState("EventFinishedState"));
	}
}
