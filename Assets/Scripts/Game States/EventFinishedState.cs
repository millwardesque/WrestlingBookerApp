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

		float talentCost = 0.0f;
		foreach (WrestlingMatch match in wrestlingEvent.matches) {
			foreach (WrestlingTeam team in match.teams) {
				foreach (Wrestler wrestler in team.wrestlers) {
					talentCost += wrestler.perMatchCost;
				}
			}
		}

		Debug.Log ("@TODO: Add merchandise and food sales");

		wrestlingEvent.revenue = ticketRevenue - venueCost - eventTypeCost - talentCost;
		gameManager.OnWrestlingEventUpdated();

		gameManager.GetPlayerCompany().money += wrestlingEvent.revenue;
		gameManager.OnCompanyUpdated();

		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		string reportText = string.Format("{0}\n{1} tickets @ ${2} = ${3}\n{4} costs: -${5}\nVenue: -${6}\nTalent: -${7}\nTotal profit = ${8}\n\nOverall rating: {9} / 10", wrestlingEvent.eventName, wrestlingEvent.ticketsSold, wrestlingEvent.ticketPrice, ticketRevenue, wrestlingEvent.Type.typeName, eventTypeCost, venueCost, talentCost, wrestlingEvent.revenue, Mathf.RoundToInt(wrestlingEvent.Rating * 10.0f));
		dialog.Initialize("Event Report", reportText, new UnityAction(OnAcknowledgeReport));
	}

	void OnAcknowledgeReport() {
		gameManager.SetState(gameManager.FindState("IdleGameState"));
	}
}
