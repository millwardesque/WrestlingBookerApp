using UnityEngine;
using System.Collections;

public class WaitGameState : GameState {
	public float waitTime = 0.0f;
	public GameState nextState;
	bool canChangeState = false;

	public void Initialize(float waitTime, GameState nextState) {
		this.waitTime = waitTime;
		this.nextState = nextState;
	}

	public override void OnEnter(GameManager gameManager) {
		StartCoroutine("Wait");
	}

	public override void OnUpdate(GameManager gameManager) {
		if (canChangeState) {
			if (nextState == null) {
				Debug.LogError ("Wait Game State is about to transition to a null state. This is probably not what you want");
			}
			gameManager.SetState(nextState);
		}
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds(waitTime);
		canChangeState = true;
	}
}
