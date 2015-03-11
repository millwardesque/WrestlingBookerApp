using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public float stateChangeDelay = 5.0f;
	public WrestlingEvent wrestlingEventPrefab;

	GameState[] availableGameStates;
	Stack<GameState> stateStack = new Stack<GameState>();
	VenueManager venueManager;
	EventTypeManager eventTypeManager;
	WrestlerManager wrestlerManager;
	WrestlingMatchTypeManager matchTypeManager;
	WrestlingMatchFinishManager matchFinishManager;
	CompanyManager companyManager;
	GUIManager guiManager;
	WrestlingEvent currentEvent;

	Company playerCompany;

	public static GameManager Instance;

	// Use this for initialization
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);

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

			companyManager = GameObject.FindObjectOfType<CompanyManager>();
			if (null == companyManager) {
				Debug.LogError("Error starting Game Manager: No company manager was found.");
			}

			if (null == wrestlingEventPrefab) {
				Debug.LogError("Error starting Game Manager: No wrestling event prefab was found.");
			}

			availableGameStates = Resources.LoadAll<GameState>("Game States");
		}
		else {
			Destroy (gameObject);
		}
	}

	public string GameID {
		get { return "1"; }
	}

	void Start()  {
		// Load the wrestlers
		WrestlerManager.Instance.LoadWrestlers();
	
		// Load the company.
		playerCompany = companyManager.CreateCompany ();
		if (playerCompany.IsSaved("playerCompany")) {
			playerCompany.Load("playerCompany");
			GetGUIManager().GetGameInfoPanel().UpdateCompanyStatus(playerCompany);
			ReplaceState(FindState("IdleGameState"));
		}
		else {
			StartAtPhase0();
		}
	}

	void StartAtPhase0() {
		UpdatePhase();
		GameState startState = FindState ("NameCompanyGameState");
		startState.SetTransition("FINISHED", OnFinishedPhase0Step);
		ReplaceState (startState);
		
		GetGUIManager().HideStatusPanel();
	}

	// Update is called once per frame
	void Update () {
		if (stateStack.Count > 0) {
			stateStack.Peek().OnUpdate(this);
		}
	}

	public void CreateNewEventAttempt() {
		if (currentEvent == null) {
			CreateNewEvent();
		}
		else {
			ConfirmState confirmState = FindState("ConfirmState") as ConfirmState;
			confirmState.Initialize("Stop current event?", "You can only have one active event at a time. Would you like to stop work on your current event?", new UnityAction(OnOkToCancelEvent), new UnityAction(OnCancelToCancelEvent));
			PushState (confirmState);
		}
	}

	void OnOkToCancelEvent() {
		PopState();
		CreateNewEvent ();
	}

	void OnCancelToCancelEvent() {
		PopState();
	}

	public void PushState(GameState newState) {
		if (stateStack.Count > 0) {
			stateStack.Peek().OnPause(this);
		}
		stateStack.Push(newState);
		newState.OnEnter(this);
	}

	public void PopState() {
		if (stateStack.Count > 0) {
			GameState oldState = stateStack.Pop();
			oldState.OnExit(this);
			Destroy (oldState.gameObject);
		}

		if (stateStack.Count > 0) {
			stateStack.Peek().OnUnpause(this);
		}
	}

	public void ReplaceState(GameState newState) {
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

	public void GoToMainMenu() {
		Application.LoadLevel("Main Menu");
	}


	public GameState GetDelayedGameState(GameState state, bool canSellTickets, float delayTime) {
		WaitGameState waitState = FindState ("WaitGameState") as WaitGameState;
		waitState.Initialize(delayTime, state, canSellTickets);
		return waitState;
	}

	public void ClearSavedData() {
		playerCompany.DeleteSaved("playerCompany");
		GameObject.Destroy(playerCompany.gameObject);
		playerCompany = companyManager.CreateCompany();

		venueManager.ClearSavedData();
		wrestlerManager.ClearSavedData();
		StartAtPhase0();
	}

	public void UpdatePhase() {
		if (GetPhase() == -1) {
			float startingMoney = 10000.0f;

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
			newState.SetTransition("FINISHED", PopState);
			PushState (newState);
		}
		else if (GetPhase() == 1 && playerCompany.money > 1000000) {
			playerCompany.isInAlliance = true;
			playerCompany.maxRosterSize = 8;
			playerCompany.phase++;
			OnCompanyUpdated();

			GameState newState = FindState("Phase1FinishedState");
			newState.SetTransition("FINISHED", SetIdleState);
			ReplaceState (newState);
		}
		else if (GetPhase() == 2 && playerCompany.Popularity > 0.5 && playerCompany.money > 20000000) {
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
		hireState.SetTransition("FINISHED", PopState);
		PushState (hireState);
	}

	public void FireWrestler() {
		GameState fireState = FindState("FireWrestlerState");
		fireState.SetTransition("FINISHED", PopState);
		PushState (fireState);
	}

	void SetIdleState() {
		ReplaceState(FindState("IdleGameState"));
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

	public void OnEventFinished() {
		GameObject.Destroy(currentEvent.gameObject);
	}

	// Phase 0 callbacks
	void OnFinishedPhase0Step() {
		GameState nextState = null;

		switch (stateStack.Peek().name) {
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
		default:
			Debug.Log ("Phase 0 state '" + stateStack.Peek().name + "' not recognized.");
			break;
		}

		if (nextState != null) {
			ReplaceState (nextState);
		}
	}

	// Event creation callbacks
	void CreateNewEvent() {
		if (currentEvent != null) {
			GameObject.Destroy(currentEvent.gameObject);
		}
		
		currentEvent = Instantiate(wrestlingEventPrefab) as WrestlingEvent;
		GetGUIManager().GetStatusPanel().UpdateEventStatus(currentEvent);
		
		GameState startState = FindState("ChooseEventTypeGameState");
		startState.SetTransition("EventTypeChosen", OnFinishedEventCreationStep);
		startState.SetTransition("NoEventsAvailable", OnCancelEventCreate);
		startState.SetTransition("Cancel", OnCancelEventCreate);
		PushState(startState);
	}
	
	void OnCancelEventCreate() {
		if (currentEvent != null) {
			GameObject.Destroy(currentEvent.gameObject);
			GetGUIManager().GetStatusPanel().UpdateEventStatus(currentEvent);
		}
		PopState();
	}

	void OnFinishedEventCreationStep() {
		GameState nextState = null;
		bool canSellTickets = true;
		bool waitBetweenStates = true;
		float delayTime = stateChangeDelay;
		
		switch (stateStack.Peek().name) {
		case "ChooseEventTypeGameState":
			nextState = FindState("NameEventGameState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "NameEventGameState":
			nextState = FindState("ChooseVenueGameState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "ChooseVenueGameState":
			nextState = FindState("ConfirmState", "SellTicketsPostVenue");
			((ConfirmState)nextState).Initialize("Sell some tickets!", string.Format("Alright, we'll get the word out that we're going to be holding an event at {0}! That should sell some tickets.", currentEvent.EventVenue.venueName), OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "SellTicketsPostVenue":
			nextState = FindState("ChooseMatchesGameState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			break;
		case "ChooseMatchesGameState":
			nextState = FindState("ConfirmState", "SellTicketsPostMatches");
			((ConfirmState)nextState).Initialize("Sell some tickets!", "Now that the card is ready to go, let's get out there and some sell tickets!", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "SellTicketsPostMatches":
			nextState = FindState("SellTicketsState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			break;
		case "SellTicketsState":
			nextState = FindState("RunEventState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "RunEventState":
			nextState = FindState("EventFinishedState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "EventFinishedState":
			PopState ();

			OnEventFinished();
			UpdatePhase();
			break;
		default:
			Debug.LogError ("Event creation state '" + stateStack.Peek().name + "' not recognized.");
			break;
		}
		
		if (nextState != null) {
			if (waitBetweenStates) {
				ReplaceState(GetDelayedGameState(nextState, canSellTickets, delayTime));
			}
			else {
				ReplaceState(nextState);
			}
		}
	}
}