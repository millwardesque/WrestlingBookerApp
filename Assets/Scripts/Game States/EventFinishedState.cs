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
					wrestler.AddUsedMatchFinish(match.finish);
				}
			}

			// @TODO Calculate ad revenue.

			wrestlingEvent.EventVenue.AddSeenMatchType(match.type);
			wrestlingEvent.EventVenue.AddSeenMatchFinish(match.finish);
		}

		// @TODO Calculate PPV revenue.

		float eventRevenue = ticketRevenue + merchSales + tvAdSales + ppvSales;
		float eventCosts = venueCost + eventTypeCost + talentCost;

		wrestlingEvent.revenue = eventRevenue - eventCosts;
		gameManager.OnWrestlingEventUpdated();

		gameManager.GetPlayerCompany().money += wrestlingEvent.revenue;
		gameManager.GetPlayerCompany().AddEvent(wrestlingEvent);
		gameManager.OnCompanyUpdated();

		InfoDialog dialog = gameManager.GetGUIManager().InstantiateInfoDialog();
		string reportText = string.Format("{0} tickets @ ${1} = ${2}\n{3} buys = ${4}\n\n{5} costs: -${6}\nVenue: -${7}\nTalent: -${8}\n\nTotal profit: ${9}\n\nOverall rating: {10} / 10", 
		                                  wrestlingEvent.TicketsSold,
		                                  wrestlingEvent.ticketPrice,
		                                  ticketRevenue,
		                                  wrestlingEvent.Type.typeName,
		                                  (tvAdSales > 0 ? tvAdSales : ppvSales),
		                                  wrestlingEvent.Type.typeName,
		                                  eventTypeCost,
		                                  Mathf.RoundToInt(venueCost),
		                                  talentCost,
		                                  Mathf.RoundToInt (wrestlingEvent.revenue),
		                                  Mathf.RoundToInt(wrestlingEvent.Rating * 10.0f)
		                                 );
		dialog.Initialize(string.Format ("Event Report: {0}", wrestlingEvent.eventName), reportText, new UnityAction(OnAcknowledgeReport));
	}

	void OnAcknowledgeReport() {
		ExecuteTransition("FINISHED");
	}
}
