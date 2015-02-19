using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SellTicketsState : GameState {
	public override void OnEnter (GameManager gameManager) {
		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		dialog.Initialize("Ticket sales", string.Format("Wow! You sold {0} tickets!", gameManager.GetCurrentEvent().TicketsSold), new UnityAction(AcknowledgedTicketSales));
	}

	void AcknowledgedTicketSales() {
		ExecuteTransition("FINISHED");
	}
}
