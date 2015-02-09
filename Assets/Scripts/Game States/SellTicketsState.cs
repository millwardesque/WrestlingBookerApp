using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SellTicketsState : GameState {
	GameManager gameManager;
	float ticketsSold = 0.0f;
	public float ticketsPerSecond = 10.0f;
	public float secondsToSell = 5.0f;
	bool finishedSellingTickets = false;
	InfoDialog dialog = null;

	public override void OnEnter (GameManager gameManager) {
		this.gameManager = gameManager;
		WrestlingEvent currentEvent = gameManager.GetCurrentEvent();

		ticketsPerSecond = currentEvent.EventInterest * currentEvent.EventVenue.capacity / secondsToSell;
		StartCoroutine("SellTickets");
	}

	public override void OnUpdate(GameManager gameManager) {
		this.gameManager = gameManager;

		if (finishedSellingTickets) {
			if (dialog == null) {
				dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
			}
		}
		else {
			ticketsSold += ticketsPerSecond * Time.deltaTime * Random.Range(0.5f, 1.5f);

			if (Mathf.FloorToInt(ticketsSold) != gameManager.GetCurrentEvent().ticketsSold) {
				gameManager.GetCurrentEvent().ticketsSold = Mathf.Clamp(Mathf.FloorToInt(ticketsSold), 0, gameManager.GetCurrentEvent().EventVenue.capacity);

				gameManager.OnWrestlingEventUpdated();
			}
		}
	}

	void AcknowledgedTicketSales() {
		gameManager.SetState(gameManager.FindState("RunEventState"));
	}

	IEnumerator SellTickets() {
		yield return new WaitForSeconds(secondsToSell);
		finishedSellingTickets = true;
	}
}
