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

	float startingMoney = 10000.0f;

	// Use this for initialization
	void Awake () {
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
	}

	void Start()  {
		// Load the company.
		playerCompany = Instantiate(companyPrefab) as Company;
		if (playerCompany.IsSaved("playerCompany")) {
			playerCompany.Load("playerCompany");
			GetGUIManager().GetGameInfoPanel().UpdateCompanyStatus(playerCompany);
			SetState(FindState("IdleGameState"));
		}
		else {
			StartAtPhase0();
		}
	}

	void StartAtPhase0() {
		UpdatePhase();
		GameState startState = FindState ("NameCompanyGameState");
		startState.SetTransition("FINISHED", OnFinishedPhase0Step);
		SetState (startState);
		
		GetGUIManager().HideStatusPanel();
	}

	// Update is called once per frame
	void Update () {
		if (state != null) {
			state.OnUpdate(this);
		}
	}

	public void CreateNewEvent() {
		string startStateName = "ChooseEventTypeGameState";
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
				GameState newState = Instantiate(availableGameStates[i]) as GameState;
				newState.name = stateName;
				return newState;
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
		GameObject.Destroy(playerCompany.gameObject);
		playerCompany = Instantiate(companyPrefab) as Company;

		StartAtPhase0();
	}

	public void UpdatePhase() {
		if (GetPhase() == -1) {
			playerCompany.money = startingMoney;
			playerCompany.maxRosterSize = 4;
			playerCompany.isInAlliance = false;
			playerCompany.phase++;

			playerCompany.unlockedVenues.Add (venueManager.GetVenue ("Civic Center"));
			playerCompany.unlockedMatchTypes.Add (matchTypeManager.GetMatchType("Standard"));
			playerCompany.unlockedMatchTypes.Add (matchTypeManager.GetMatchType("No DQ"));
			OnCompanyUpdated();
		}
		else if (GetPhase() == 0 && playerCompany.eventHistory.Count >= 1) {
			playerCompany.maxRosterSize = 6;
			playerCompany.isInAlliance = false;
			playerCompany.phase++;
			OnCompanyUpdated();

			GameState newState = FindState("Phase0FinishedState");
			newState.SetTransition("FINISHED", SetIdleState);
			SetState (newState);
		}
		else if (GetPhase() == 1 && playerCompany.Popularity > 0.5 && playerCompany.money > 1000000) {
			playerCompany.isInAlliance = true;
			playerCompany.maxRosterSize = 8;
			playerCompany.phase++;
			OnCompanyUpdated();

			GameState newState = FindState("Phase1FinishedState");
			newState.SetTransition("FINISHED", SetIdleState);
			SetState (newState);
		}
		else if (GetPhase() == 2 && playerCompany.Popularity > 0.7 && playerCompany.money > 20000000) {
			playerCompany.isInAlliance = true;
			playerCompany.maxRosterSize = 10;
			playerCompany.phase++;
			OnCompanyUpdated();
		}
		else if (GetPhase() == 3 && playerCompany.eventHistory.Count > 30) { // End game.
			playerCompany.phase++;
			OnCompanyUpdated();
		}
	}

	public void HireWrestlers() {
		GameState hireState = FindState("HireWrestlersState");
		hireState.SetTransition("FINISHED", SetIdleState);
		SetState (hireState);
	}

	void SetIdleState() {
		SetState(FindState("IdleGameState"));
	}

	public int GetPhase() {
		return GetPlayerCompany().phase;
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
		GetGUIManager().GetGameInfoPanel().UpdateCompanyStatus(playerCompany);
	}

	public void OnTimeUpdated(TimeManager timeManager) {
		GetGUIManager().GetGameInfoPanel().UpdateTime(timeManager);
	}

	// Phase 0 callbacks
	void OnFinishedPhase0Step() {
		GameState nextState = null;

		Debug.Log ("Finished '" + state.name + "'");

		switch (state.name) {
		case "NameCompanyGameState":
			nextState = FindState ("Phase0IntroGameState");
			nextState.SetTransition("FINISHED", OnFinishedPhase0Step);
			break;
		case "Phase0IntroGameState":
			nextState = FindState ("HireWrestlersState");
			nextState.SetTransition("FINISHED", OnFinishedPhase0Step);
			break;
		case "HireWrestlersState":
			nextState = FindState ("Phase0CreateEventIntroState");
			nextState.SetTransition("FINISHED", OnFinishedPhase0Step);
			break;
		case "Phase0CreateEventIntroState":
			nextState = FindState ("IdleGameState");
			break;
		}

		if (nextState != null) {
			SetState (nextState);
		}
	}
}
