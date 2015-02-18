using UnityEngine;
using System.Collections;

public class WaitGameState : GameState {
	public float waitTime = 0.0f;
	public GameState nextState;

	public void Initialize(float waitTime, GameState nextState) {
		this.waitTime = waitTime;
		this.nextState = nextState;
	}

	public override void OnUpdate(GameManager gameManager) {
		if (waitTime > 0.0f) {
			waitTime -= Time.deltaTime;
		}
		else {
			if (nextState == null) {
				Debug.LogError ("Wait Game State is about to transition to a null state. This is probably not what you want");
			}
			gameManager.ReplaceState(nextState);
		}
	}
}
