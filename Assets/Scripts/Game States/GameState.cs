using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {
	Dictionary<string, System.Action> transitions = new Dictionary<string, System.Action>();

	public virtual void OnEnter(GameManager gameManager) { }
	public virtual void OnExit(GameManager gameManager) { }
	public virtual void OnUpdate(GameManager gameManager) { }

	public void SetTransition(string transitionName, System.Action action) {
		transitions.Add(transitionName, action);
	}

	protected void ExecuteTransition(string transitionName) {
		if (!transitions.ContainsKey(transitionName)) {
			throw new UnityException("Unable to execute transition on game state '" + this.name + "': Transition name '" + transitionName + "' doesn't exist");
		}
		transitions[transitionName]();
	}
}
