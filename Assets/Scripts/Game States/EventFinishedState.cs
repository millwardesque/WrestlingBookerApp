using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventFinishedState : GameState {

	public override void OnEnter (GameManager gameManager) {
		WrestlingEvent wrestlingEvent = gameManager.GetCurrentEvent();

		float ticketRevenue = wrestlingEvent.ticketPrice * wrestlingEvent.TicketsSold;
		float merchSales = 0f;
		float ppvSales = 0f;
		float tvAdSales = 0f;

		float venueCost = wrestlingEvent.EventVenue.GetVenueCost(wrestlingEvent);
		float eventTypeCost = wrestlingEvent.Type.cost;
		float talentCost = 0.0f;

		// Calculate per match / per wrestler costs and revenue.
		foreach (WrestlingMatch match in wrestlingEvent.matches) {
			foreach (WrestlingTeam team in match.teams) {
				foreach (Wrestler wrestler in team.wrestlers) {
					talentCost += wrestler.perMatchCost;

					// @TODO Calculate merch sales.

					wrestler.AddUsedMatchType(match.type);
				}
			}

			// @TODO Calculate ad revenue.

			wrestlingEvent.EventVenue.AddSeenMatchType(match.type);
			wrestlingEvent.EventVenue.AddSeenMatchFinish(match.finish);
		}

		float eventRevenue = ticketRevenue + merchSales + tvAdSales + ppvSales;
		float eventCosts = venueCost + eventTypeCost + talentCost;

		wrestlingEvent.revenue = eventRevenue - eventCosts;
		gameManager.OnWrestlingEventUpdated();

		gameManager.GetPlayerCompany().money += wrestlingEvent.revenue;
		gameManager.GetPlayerCompany().AddEvent(wrestlingEvent);
		gameManager.OnCompanyUpdated();

		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		string reportText = string.Format("{0}\n{1} tickets @ ${2} = ${3}\n{4} costs: -${5}\nVenue: -${6}\nTalent: -${7}\n{8} buys = ${9}\nTotal profit = ${10}\n\nOverall rating: {11} / 10", 
		                                  wrestlingEvent.eventName,
		                                  wrestlingEvent.TicketsSold,
		                                  wrestlingEvent.ticketPrice,
		                                  ticketRevenue,
		                                  wrestlingEvent.Type.typeName,
		                                  eventTypeCost,
		                                  venueCost,
		                                  talentCost,
		                                  wrestlingEvent.Type.typeName,
		                                  (tvAdSales > 0 ? tvAdSales : ppvSales),
		                                  wrestlingEvent.revenue,
		                                  Mathf.RoundToInt(wrestlingEvent.Rating * 10.0f)
		                                 );
		dialog.Initialize("Event Report", reportText, new UnityAction(OnAcknowledgeReport));
	}

	void OnAcknowledgeReport() {
		ExecuteTransition("FINISHED");
	}
}
