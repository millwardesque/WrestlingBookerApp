using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateMachine : MonoBehaviour {
	GameState[] availableGameStates;
	Stack<GameState> stateStack = new Stack<GameState>();


	void Update() {
		if (stateStack.Count > 0) {
			stateStack.Peek().OnUpdate(GameManager.Instance);
		}
	}

	public GameState CurrentState {
		get { return stateStack.Peek(); }
	}

	public void SetAvailableGameStates(GameState[] gameStates) {
		availableGameStates = gameStates;
	}
	
	public void PushState(GameState newState) {
		if (stateStack.Count > 0) {
			stateStack.Peek().OnPause(GameManager.Instance);
		}
		stateStack.Push(newState);
		newState.OnEnter(GameManager.Instance);
	}

	public void PopState() {
		if (stateStack.Count > 0) {
			GameState oldState = stateStack.Pop();
			oldState.OnExit(GameManager.Instance);
			Destroy (oldState.gameObject);
		}

		if (stateStack.Count > 0) {
			stateStack.Peek().OnUnpause(GameManager.Instance);
		}
	}

	public void ReplaceState(GameState newState) {
		if (null == newState) {
			return;
		}

		PopState ();
		PushState (newState);
	}

	public void ReplaceState(string stateName) {
		GameState newState = FindState(stateName);
		if (null == newState) {
			return;
		}
		
		PopState ();
		PushState (newState);
	}

	public GameState FindState(string stateName, string newName = "") {
		for (int i = 0; i < availableGameStates.Length; ++i) {
			if (availableGameStates[i].name == stateName) {
				GameState newState = Instantiate(availableGameStates[i]) as GameState;

				if (newName != "") {
					newState.name = newName;
				}
				else {
					newState.name = stateName;
				}
				return newState;
			}
		}
		Debug.LogError("Couldn't find game state '" + stateName + "'");
		return null;
	}


}
