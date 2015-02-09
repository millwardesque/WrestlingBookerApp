using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public GameState[] availableGameStates;
	public float stateChangeDelay = 1.0f;
	public WrestlingEvent wrestlingEventPrefab;
	public Company companyPrefab;

	GameState state;
	VenueManager venueManager;
	EventTypeManager eventTypeManager;
	WrestlerManager wrestlerManager;
	WrestlingMatchTypeManager matchTypeManager;
	WrestlingMatchFinishManager matchFinishManager;
	GUIManager guiManager;
	WrestlingEvent currentEvent;
	Company playerCompany;

	// Use this for initialization
	void Start () {
		GameObject guiManagerObj = GameObject.FindGameObjectWithTag("GUI Manager");
		if (null == guiManagerObj || null == guiManagerObj.GetComponent<GUIManager>()) {
			Debug.LogError("Error starting Game Manager: No tagged GUI Manager was found.");
		}
		guiManager = guiManagerObj.GetComponent<GUIManager>();

		venueManager = GameObject.FindObjectOfType<VenueManager>();
		if (null == venueManager) {
			Debug.LogError("Error starting Game Manager: No venue manager was found.");
		}

		eventTypeManager = GameObject.FindObjectOfType<EventTypeManager>();
		if (null == eventTypeManager) {
			Debug.LogError("Error starting Game Manager: No event type manager was found.");
		}

		wrestlerManager = GameObject.FindObjectOfType<WrestlerManager>();
		if (null == wrestlerManager) {
			Debug.LogError("Error starting Game Manager: No wrestler manager was found.");
		}

		matchTypeManager = GameObject.FindObjectOfType<WrestlingMatchTypeManager>();
		if (null == matchTypeManager) {
			Debug.LogError("Error starting Game Manager: No wrestling match type manager was found.");
		}

		matchFinishManager = GameObject.FindObjectOfType<WrestlingMatchFinishManager>();
		if (null == matchFinishManager) {
			Debug.LogError("Error starting Game Manager: No wrestling match finish manager was found.");
		}

		if (null == wrestlingEventPrefab) {
			Debug.LogError("Error starting Game Manager: No wrestling event prefab was found.");
		}

		if (null == companyPrefab) {
			Debug.LogError("Error starting Game Manager: No company prefab was found.");
		}

		string startStateName = "IdleGameState";

		playerCompany = Instantiate(companyPrefab) as Company;

		if (playerCompany.IsSaved("playerCompany")) {
			playerCompany.Load("playerCompany");
			GetGUIManager().GetStatusPanel().UpdateCompanyStatus(playerCompany);
		}
		else {
			playerCompany.money = 5000.0f;
			startStateName = "NameCompanyGameState";
			GetGUIManager().HideStatusPanel();
		}
		
		SetState (FindState(startStateName));
	}
	
	// Update is called once per frame
	void Update () {
		if (state != null) {
			state.OnUpdate(this);
		}
	}

	public void CreateNewEvent() {
		string startStateName = "NameEventGameState";
		SetState(FindState(startStateName));
		currentEvent = Instantiate(wrestlingEventPrefab) as WrestlingEvent;
		GetGUIManager().GetStatusPanel().UpdateEventStatus(currentEvent);
	}

	public void SetState(GameState newState) {
		if (null == newState) {
			return;
		}

		if (null != state) {
			state.OnExit(this);
			Destroy (state.gameObject);
		}

		state = newState;
		state.OnEnter(this);
	}

	public GameState FindState(string stateName) {
		for (int i = 0; i < availableGameStates.Length; ++i) {
			if (availableGameStates[i].name == stateName) {
				return Instantiate(availableGameStates[i]) as GameState;
			}
		}
		Debug.LogError("Couldn't find game state '" + stateName + "'");
		return null;
	}

	public void GoToMainMenu() {
		Application.LoadLevel("Main Menu");
	}


	public GameState GetDelayedGameState(GameState state) {
		WaitGameState waitState = FindState ("WaitGameState") as WaitGameState;
		waitState.Initialize(stateChangeDelay, state);
		return waitState;
	}

	public void ClearSavedData() {
		playerCompany.DeleteSaved("playerCompany");
		playerCompany = Instantiate(companyPrefab) as Company;
		playerCompany.money = 5000.0f;
		GetGUIManager().HideStatusPanel();
		
		SetState (FindState("NameCompanyGameState"));
	}

	public void HireWreslters() {
		SetState (FindState("HireWrestlersState"));
	}

	public GUIManager GetGUIManager() {
		return guiManager;
	}

	public VenueManager GetVenueManager() {
		return venueManager;
	}

	public EventTypeManager GetEventTypeManager() {
		return eventTypeManager;
	}

	public WrestlerManager GetWrestlerManager() {
		return wrestlerManager;
	}

	public WrestlingMatchTypeManager GetMatchTypeManager() {
		return matchTypeManager;
	}

	public WrestlingMatchFinishManager GetMatchFinishManager() {
		return matchFinishManager;
	}
	
	public WrestlingEvent GetCurrentEvent() {
		return currentEvent;
	}

	public Company GetPlayerCompany() {
		return playerCompany;
	}

	public void OnWrestlingEventUpdated() {
		GetGUIManager().GetStatusPanel().UpdateEventStatus(currentEvent);
	}

	public void OnCompanyUpdated() {
		playerCompany.Save("playerCompany");
		GetGUIManager().GetStatusPanel().UpdateCompanyStatus(playerCompany);
	}
}
