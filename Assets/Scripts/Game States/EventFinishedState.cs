using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventFinishedState : GameState {
	GameManager gameManager;

	public override void OnEnter (GameManager gameManager) {
		this.gameManager = gameManager;

		WrestlingEvent wrestlingEvent = gameManager.GetCurrentEvent();
		float ticketRevenue = wrestlingEvent.ticketPrice * wrestlingEvent.ticketsSold;
		float venueCost = wrestlingEvent.EventVenue.GetVenueCost(wrestlingEvent);
		float eventTypeCost = wrestlingEvent.Type.cost;
	
		Debug.Log ("@TODO: Add merchandise and food sales");
		Debug.Log ("@TODO: Subtract talent cost");

		wrestlingEvent.revenue = ticketRevenue - venueCost - eventTypeCost;
		gameManager.OnWrestlingEventUpdated();

		gameManager.GetPlayerCompany().money += wrestlingEvent.revenue;
		gameManager.OnCompanyUpdated();

		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		string reportText = string.Format("{0}\n{1} tickets @ ${2} = ${3}\n{4} costs: -${5}\nVenue: -${6}\nTotal profit = ${7}", wrestlingEvent.eventName, wrestlingEvent.ticketsSold, wrestlingEvent.ticketPrice, ticketRevenue, wrestlingEvent.Type.typeName, eventTypeCost, venueCost, wrestlingEvent.revenue);
		dialog.Initialize("Event Report", reportText, new UnityAction(OnAcknowledgeReport));
	}

	void OnAcknowledgeReport() {
		gameManager.SetState(gameManager.FindState("IdleGameState"));
	}
}
