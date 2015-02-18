using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SellTicketsState : GameState {
	float ticketsSold = 0.0f;
	public float ticketsPerSecond = 10.0f;
	public float secondsToSell = 5.0f;
	bool finishedSellingTickets = false;
	InfoDialog dialog = null;

	public override void OnEnter (GameManager gameManager) {
		WrestlingEvent currentEvent = gameManager.GetCurrentEvent();

		ticketsPerSecond = currentEvent.EventInterest * currentEvent.EventVenue.capacity / secondsToSell;
	}

	public override void OnUpdate(GameManager gameManager) {
		if (finishedSellingTickets) {
			if (dialog == null) {
				dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
				dialog.Initialize("Ticket sales", string.Format("Wow! You sold {0} tickets!", gameManager.GetCurrentEvent().ticketsSold), new UnityAction(AcknowledgedTicketSales));
			}
		}
		else {
			secondsToSell -= Time.deltaTime;
			if (secondsToSell <= 0.0f) {
				finishedSellingTickets = true;
			}
			ticketsSold += ticketsPerSecond * Time.deltaTime * Random.Range(0.5f, 1.5f);

			if (Mathf.FloorToInt(ticketsSold) != gameManager.GetCurrentEvent().ticketsSold) {
				gameManager.GetCurrentEvent().ticketsSold = Mathf.Clamp(Mathf.FloorToInt(ticketsSold), 0, gameManager.GetCurrentEvent().EventVenue.capacity);

				gameManager.OnWrestlingEventUpdated();
			}
		}
	}

	void AcknowledgedTicketSales() {
		ExecuteTransition("FINISHED");
	}
}
