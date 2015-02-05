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
		ticketsPerSecond += currentEvent.EventVenue.popularity * currentEvent.EventVenue.capacity / secondsToSell;

		StartCoroutine("SellTickets");
	}

	public override void OnUpdate(GameManager gameManager) {
		this.gameManager = gameManager;

		if (finishedSellingTickets) {
			if (dialog == null) {
				dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
				dialog.Initialize("Ticket sales", string.Format("Wow! You sold {0} tickets!", gameManager.GetCurrentEvent().ticketsSold), new UnityAction(AcknowledgedTicketSales));
			}
		}
		else {
			ticketsSold += ticketsPerSecond * Time.deltaTime * Random.Range(0.5f, 1.5f);
			ticketsSold = Mathf.Clamp(ticketsSold, 0.0f, gameManager.GetCurrentEvent().EventVenue.capacity);

			if (Mathf.FloorToInt(ticketsSold) != gameManager.GetCurrentEvent().ticketsSold) {
				gameManager.GetCurrentEvent().ticketsSold = Mathf.FloorToInt(ticketsSold);
			}
			gameManager.OnWrestlingEventUpdated();
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
