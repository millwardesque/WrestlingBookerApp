using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	public virtual void OnEnter(GameManager gameManager) { }
	public virtual void OnExit(GameManager gameManager) { }
	public virtual void OnUpdate(GameManager gameManager) { }
}
