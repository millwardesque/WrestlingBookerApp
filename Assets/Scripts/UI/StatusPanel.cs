using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusPanel : MonoBehaviour {
	public Text companyName;
	public Text companyMoney;
	public Text eventName;
	public Text eventType;
	public Text eventVenue;
	public Text ticketsSold;
	public Text revenue;

	public void UpdateEventStatus(WrestlingEvent wrestlingEvent) {
		eventName.text = wrestlingEvent.eventName;
		eventType.text = "Type: " + (wrestlingEvent.Type != null ? wrestlingEvent.Type.typeName : "<TBD>");
		eventVenue.text = "Venue: " + (wrestlingEvent.EventVenue != null ? wrestlingEvent.EventVenue.venueName : "<TBD>");
		ticketsSold.text = "Tickets: " + (wrestlingEvent.ticketsSold >= 0 ? wrestlingEvent.ticketsSold.ToString() : "<TBD>");
		revenue.text = string.Format("${0}", wrestlingEvent.revenue);
	}

	public void UpdateCompanyStatus(Company company) {
		this.companyName.text = company.companyName;
		this.companyMoney.text = string.Format ("${0}", company.money);
	}
}
