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
		float eventTypeSales = gameManager.GetPlayerCompany().Popularity * wrestlingEvent.Type.ticketsToExternalMultiplier * wrestlingEvent.Type.externalRevenuePerUser * wrestlingEvent.ticketsSold;

		float talentCost = 0.0f;
		foreach (WrestlingMatch match in wrestlingEvent.matches) {
			foreach (WrestlingTeam team in match.teams) {
				foreach (Wrestler wrestler in team.wrestlers) {
					talentCost += wrestler.perMatchCost;
				}
			}
		}

		wrestlingEvent.revenue = ticketRevenue + eventTypeSales - venueCost - eventTypeCost - talentCost;
		gameManager.OnWrestlingEventUpdated();

		gameManager.GetPlayerCompany().money += wrestlingEvent.revenue;
		gameManager.GetPlayerCompany().AddEvent(wrestlingEvent);
		gameManager.OnCompanyUpdated();

		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		string reportText = string.Format("{0}\n{1} tickets @ ${2} = ${3}\n{4} costs: -${5}\nVenue: -${6}\nTalent: -${7}\n{8} buys = ${9}\nTotal profit = ${10}\n\nOverall rating: {11} / 10", 
		                                  wrestlingEvent.eventName,
		                                  wrestlingEvent.ticketsSold,
		                                  wrestlingEvent.ticketPrice,
		                                  ticketRevenue,
		                                  wrestlingEvent.Type.typeName,
		                                  eventTypeCost,
		                                  venueCost,
		                                  talentCost,
		                                  wrestlingEvent.Type.typeName,
		                                  eventTypeSales,
		                                  wrestlingEvent.revenue,
		                                  Mathf.RoundToInt(wrestlingEvent.Rating * 10.0f)
		                                 );
		dialog.Initialize("Event Report", reportText, new UnityAction(OnAcknowledgeReport));
	}

	void OnAcknowledgeReport() {
		gameManager.OnEventFinished();
		gameManager.ReplaceState(gameManager.FindState("IdleGameState"));
		gameManager.UpdatePhase();
	}
}
