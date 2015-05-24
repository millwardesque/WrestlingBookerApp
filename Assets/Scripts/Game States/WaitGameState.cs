using UnityEngine;
using System.Collections;

public class WaitGameState : GameState {
	public float waitTime = 0.0f;
	public GameState nextState;

	bool canSellTickets = false;
	float ticketsPerSecond = 0.0f;
	float ticketsSold = 0.0f;

	public void Initialize(float waitTime, GameState nextState, bool canSellTickets) {
		this.waitTime = waitTime;
		this.nextState = nextState;
		this.canSellTickets = canSellTickets;
	}

	public override void OnEnter(GameManager gameManager) {
		WrestlingEvent currentEvent = gameManager.GetCurrentEvent();

		if (canSellTickets && waitTime > 0 && currentEvent.EventVenue != null) {
			float ticketsSold = currentEvent.EventInterest * currentEvent.EventVenue.capacity;
			Debug.Log (string.Format("Tickets: {0} x {1} = {2}", currentEvent.EventInterest, currentEvent.EventVenue.capacity, ticketsSold)); 
			ticketsPerSecond = ticketsSold / waitTime;
		}
		else {
			ticketsPerSecond = 0;
		}
	}

	public override void OnUpdate(GameManager gameManager) {
		if (waitTime > 0.0f) {
			waitTime -= Time.deltaTime;

			// Sell tickets
			if (canSellTickets) {
				ticketsSold += ticketsPerSecond * Time.deltaTime * Random.Range(0.5f, 1.5f);
				if (Mathf.FloorToInt(ticketsSold) != gameManager.GetCurrentEvent().TicketsSold) {
					gameManager.GetCurrentEvent().TicketsSold += Mathf.FloorToInt(ticketsSold);
					ticketsSold -=  Mathf.Floor(ticketsSold);

					gameManager.OnWrestlingEventUpdated();
				}
			}

			// @TODO Check for random events
		}
		else {
			if (nextState == null) {
				Debug.LogError ("Wait Game State is about to transition to a null state. This is probably not what you want");
			}
			gameManager.StateMachine.ReplaceState(nextState);
		}
	}
}
