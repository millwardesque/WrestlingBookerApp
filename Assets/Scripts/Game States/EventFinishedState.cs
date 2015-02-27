using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventFinishedState : GameState {

	public override void OnEnter (GameManager gameManager) {
		WrestlingEvent wrestlingEvent = gameManager.GetCurrentEvent();
		float ticketRevenue = wrestlingEvent.ticketPrice * wrestlingEvent.TicketsSold;
		float venueCost = wrestlingEvent.EventVenue.GetVenueCost(wrestlingEvent);
		float eventTypeCost = wrestlingEvent.Type.cost;
		float eventTypeSales = gameManager.GetPlayerCompany().Popularity * wrestlingEvent.Type.ticketsToExternalMultiplier * wrestlingEvent.Type.externalRevenuePerUser * wrestlingEvent.TicketsSold;

		float talentCost = 0.0f;
		foreach (WrestlingMatch match in wrestlingEvent.matches) {
			foreach (WrestlingTeam team in match.teams) {
				foreach (Wrestler wrestler in team.wrestlers) {
					talentCost += wrestler.perMatchCost;
					wrestler.AddUsedMatchType(match.type);
				}
			}

			wrestlingEvent.EventVenue.AddSeenMatchType(match.type);
			wrestlingEvent.EventVenue.AddSeenMatchFinish(match.finish);
		}

		wrestlingEvent.revenue = ticketRevenue + eventTypeSales - venueCost - eventTypeCost - talentCost;
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
		                                  eventTypeSales,
		                                  wrestlingEvent.revenue,
		                                  Mathf.RoundToInt(wrestlingEvent.Rating * 10.0f)
		                                 );
		dialog.Initialize("Event Report", reportText, new UnityAction(OnAcknowledgeReport));
	}

	void OnAcknowledgeReport() {
		ExecuteTransition("FINISHED");
	}
}
