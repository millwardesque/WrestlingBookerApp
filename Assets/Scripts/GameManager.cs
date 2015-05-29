using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public float stateChangeDelay = 5.0f;
	public WrestlingEvent wrestlingEventPrefab;

	GameStateMachine stateMachine;
	EventTypeManager eventTypeManager;
	WrestlingMatchTypeManager matchTypeManager;
	WrestlingMatchFinishManager matchFinishManager;
	GUIManager guiManager;
	WrestlingEvent currentEvent;

	Company playerCompany;

	public static GameManager Instance;

	public List<WrestlingMatchType> StartingMatchTypes {
		get {
			List<WrestlingMatchType> matchTypes = new List<WrestlingMatchType>();
			matchTypes.Add(matchTypeManager.GetMatchType("Standard"));
			matchTypes.Add(matchTypeManager.GetMatchType("No DQ"));
			return matchTypes;
		}
	}

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

			eventTypeManager = GameObject.FindObjectOfType<EventTypeManager>();
			if (null == eventTypeManager) {
				Debug.LogError("Error starting Game Manager: No event type manager was found.");
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

			stateMachine = GetComponentInChildren<GameStateMachine>();
			if (null == stateMachine) {
				Debug.LogError("Error starting Game Manager: No child state machine was found.");
			}
			stateMachine.SetAvailableGameStates(Resources.LoadAll<GameState>("Game States"));
		}
		else {
			Destroy (gameObject);
		}
	}

	public string GameID {
		get { return "1"; }
	}

	string PlayerFilename {
		get { return GameID + ".game?tag=player"; }
	}

	void Start()  {
		WrestlerManager.Instance.LoadWrestlers();
		VenueManager.Instance.LoadVenues();

		// Load companies after wrestlers so that the rosters can be constructed properly.
		CompanyManager.Instance.LoadCompanies();
	
		// Load the company.
		if (ES2.Exists(PlayerFilename)) {
			string playerID = ES2.Load<string> (PlayerFilename);
			playerCompany = CompanyManager.Instance.GetCompany (playerID);

			if (playerCompany != null) {
				GetGUIManager().GetGameInfoPanel().UpdateCompanyStatus(playerCompany);
				StateMachine.ReplaceState("IdleGameState");
			}
			else {
				Debug.LogError("Error: Unable to load player company with ID '" + playerID + "': No such company exists.");
				StartAtPhase0();
			}
		}
		else {
			StartAtPhase0();
		}
	}

	void StartAtPhase0() {
		playerCompany = CompanyManager.Instance.CreateCompany ();
		GameState startState = StateMachine.FindState ("NameCompanyGameState");
		startState.SetTransition("FINISHED", NextPhase0Step);
		StateMachine.ReplaceState (startState);
		
		GetGUIManager().HideStatusPanel();
	}

	public void CreateNewEventAttempt() {
		if (currentEvent == null) {
			CreateNewEvent();
		}
		else {
			ConfirmState confirmState = StateMachine.FindState("ConfirmState") as ConfirmState;
			confirmState.Initialize("Stop current event?", "You can only have one active event at a time. Would you like to stop work on your current event?", new UnityAction(OnOkToCancelEvent), new UnityAction(OnCancelToCancelEvent));
			StateMachine.PushState (confirmState);
		}
	}

	void OnOkToCancelEvent() {
		StateMachine.PopState();
		CreateNewEvent ();
	}

	void OnCancelToCancelEvent() {
		StateMachine.PopState();
	}

	public void GoToMainMenu() {
		Application.LoadLevel("Main Menu");
	}


	public GameState GetDelayedGameState(GameState state, bool canSellTickets, float delayTime) {
		WaitGameState waitState = StateMachine.FindState ("WaitGameState") as WaitGameState;
		waitState.Initialize(delayTime, state, canSellTickets);
		return waitState;
	}

	public void ClearSavedData() {
		ES2.Delete(PlayerFilename);
		CompanyManager.Instance.ClearSavedData();
		VenueManager.Instance.ClearSavedData();
		WrestlerManager.Instance.ClearSavedData();

		StartAtPhase0();
	}

	public void UpdatePhase() {
		if (GetPhase() == -1) {
			float startingMoney = 10000.0f;

			playerCompany.money = startingMoney;
			playerCompany.maxRosterSize = 4;
			playerCompany.isInAlliance = false;
			playerCompany.phase++;

			playerCompany.UnlockVenue(VenueManager.Instance.GetVenue("Civic Center"));
			foreach (WrestlingMatchType type in StartingMatchTypes) {
				playerCompany.unlockedMatchTypes.Add (type);
			}

			OnCompanyUpdated();
		}
		else if (GetPhase() == 0 && playerCompany.eventHistory.Count >= 10) {
			playerCompany.maxRosterSize = 6;
			playerCompany.isInAlliance = false;
			playerCompany.phase++;
			OnCompanyUpdated();

			GameState newState = StateMachine.FindState("Phase0FinishedState");
			newState.SetTransition("FINISHED", StateMachine.PopState);
			StateMachine.PushState (newState);
		}
		else if (GetPhase() == 1 && playerCompany.money > 1000000) {
			playerCompany.isInAlliance = true;
			playerCompany.maxRosterSize = 8;
			playerCompany.phase++;
			OnCompanyUpdated();

			GameState newState = StateMachine.FindState("Phase1FinishedState");
			newState.SetTransition("FINISHED", SetIdleState);
			StateMachine.ReplaceState (newState);
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

	public void TrainWrestlers() {
		GameState trainState = StateMachine.FindState("TrainWrestlersState");
		trainState.SetTransition("FINISHED", StateMachine.PopState);
		StateMachine.PushState (trainState);
	}

	public void HireWrestlers() {
		GameState hireState = StateMachine.FindState("HireWrestlersState");
		hireState.SetTransition("FINISHED", StateMachine.PopState);
		StateMachine.PushState (hireState);
	}

	public void FireWrestler() {
		GameState fireState = StateMachine.FindState("FireWrestlerState");
		fireState.SetTransition("FINISHED", StateMachine.PopState);
		StateMachine.PushState (fireState);
	}

	void SetIdleState() {
		StateMachine.ReplaceState("IdleGameState");
	}

	public int GetPhase() {
		return GetPlayerCompany().phase;
	}

	public int GetIdealMatchCount() {
		int idealCount = 4;
		switch (GetPhase()) {
		case 0:
			idealCount = 2;
			break;
		case 1:
			idealCount = 3;
			break;
		case 2:
			idealCount = 4;
			break;
		case 3:
			idealCount = 4;
			break;
		default:
			break;
		}

		return idealCount;
	}

	public GUIManager GetGUIManager() {
		return guiManager;
	}

	public EventTypeManager GetEventTypeManager() {
		return eventTypeManager;
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

	public GameStateMachine StateMachine {
		get { return stateMachine; }
	}

	public void OnWrestlingEventUpdated() {
		GetGUIManager().GetStatusPanel().UpdateEventStatus(currentEvent);
	}

	public void OnCompanyUpdated() {
		if (playerCompany.companyName != playerCompany.name) {
			playerCompany.name = playerCompany.companyName;
		}
		SavePlayer();

		GetGUIManager().GetGameInfoPanel().UpdateCompanyStatus(playerCompany);
	}

	void SavePlayer() {
		playerCompany.Save();
		ES2.Save(playerCompany.id, PlayerFilename);
	}

	public void OnTimeUpdated(TimeManager timeManager) {
		GetGUIManager().GetGameInfoPanel().UpdateTime(timeManager);
	}

	public void OnEventFinished() {
		GameObject.Destroy(currentEvent.gameObject);
	}

	// Phase 0 callbacks
	void NextPhase0Step() {
		GameState nextState = null;

		switch (StateMachine.CurrentState.name) {
		case "NameCompanyGameState":
			CompanyManager.Instance.AddCompany(playerCompany);
			UpdatePhase(); // Update the phase now to set the player's money, etc. and save the player so that the un-named players don't get saved.

			nextState = StateMachine.FindState ("Phase0IntroGameState");
			nextState.SetTransition("FINISHED", NextPhase0Step);
			break;
		case "Phase0IntroGameState":
			nextState = StateMachine.FindState ("HireWrestlersState");
			nextState.SetTransition("FINISHED", NextPhase0Step);
			break;
		case "HireWrestlersState":
			nextState = StateMachine.FindState ("Phase0CreateEventIntroState");
			nextState.SetTransition("FINISHED", NextPhase0Step);
			break;
		case "Phase0CreateEventIntroState":
			nextState = StateMachine.FindState ("IdleGameState");
			break;
		default:
			Debug.Log ("Phase 0 state '" + StateMachine.CurrentState.name + "' not recognized.");
			break;
		}

		if (nextState != null) {
			StateMachine.ReplaceState (nextState);
		}
	}

	// Event creation callbacks
	void CreateNewEvent() {
		if (currentEvent != null) {
			GameObject.Destroy(currentEvent.gameObject);
		}
		
		currentEvent = Instantiate(wrestlingEventPrefab) as WrestlingEvent;
		GetGUIManager().GetStatusPanel().UpdateEventStatus(currentEvent);
		
		GameState startState = StateMachine.FindState("ChooseEventTypeGameState");
		startState.SetTransition("EventTypeChosen", OnFinishedEventCreationStep);
		startState.SetTransition("NoEventsAvailable", OnCancelEventCreate);
		startState.SetTransition("Cancel", OnCancelEventCreate);
		StateMachine.PushState(startState);
	}
	
	void OnCancelEventCreate() {
		if (currentEvent != null) {
			GameObject.Destroy(currentEvent.gameObject);
			GetGUIManager().GetStatusPanel().UpdateEventStatus(currentEvent);
		}
		StateMachine.PopState();
	}

	void OnFinishedEventCreationStep() {
		GameState nextState = null;
		bool canSellTickets = true;
		bool waitBetweenStates = true;
		float delayTime = stateChangeDelay;
		
		switch (StateMachine.CurrentState.name) {
		case "ChooseEventTypeGameState":
			nextState = StateMachine.FindState("NameEventGameState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "NameEventGameState":
			nextState = StateMachine.FindState("ChooseVenueGameState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "ChooseVenueGameState":
			nextState = StateMachine.FindState("ConfirmState", "SellTicketsPostVenue");
			((ConfirmState)nextState).Initialize("Sell some tickets!", string.Format("Alright, we'll get the word out that we're going to be holding an event at {0}! That should sell some tickets.", currentEvent.EventVenue.venueName), OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "SellTicketsPostVenue":
			nextState = StateMachine.FindState("ChooseMatchesGameState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			break;
		case "ChooseMatchesGameState":
			nextState = StateMachine.FindState("ConfirmState", "SellTicketsPostMatches");
			((ConfirmState)nextState).Initialize("Sell some tickets!", "Now that the card is ready to go, let's get out there and some sell tickets!", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "SellTicketsPostMatches":
			nextState = StateMachine.FindState("SellTicketsState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			break;
		case "SellTicketsState":
			nextState = StateMachine.FindState("RunEventState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "RunEventState":
			nextState = StateMachine.FindState("EventFinishedState");
			nextState.SetTransition("FINISHED", OnFinishedEventCreationStep);
			waitBetweenStates = false;
			break;
		case "EventFinishedState":
			StateMachine.PopState ();

			OnEventFinished();
			UpdatePhase();
			break;
		default:
			Debug.LogError ("Event creation state '" + StateMachine.CurrentState.name + "' not recognized.");
			break;
		}
		
		if (nextState != null) {
			if (waitBetweenStates) {
				StateMachine.ReplaceState(GetDelayedGameState(nextState, canSellTickets, delayTime));
			}
			else {
				StateMachine.ReplaceState(nextState);
			}
		}
	}
}