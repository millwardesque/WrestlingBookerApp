using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusPanel : MonoBehaviour {
	public Text eventName;
	public Text eventType;
	public Text eventVenue;
	public Text ticketsSold;
	public Text revenue;

	public void UpdateEventStatus(WrestlingEvent wrestlingEvent) {
		eventName.text = wrestlingEvent.eventName;
		eventType.text = "Type: " + (wrestlingEvent.Type != null ? wrestlingEvent.Type.typeName : "<TBD>");
		eventVenue.text = "Venue: " + (wrestlingEvent.EventVenue != null ? wrestlingEvent.EventVenue.venueName : "<TBD>");
		ticketsSold.text = (wrestlingEvent.ticketsSold >= 0 && wrestlingEvent.EventVenue != null ? string.Format ("Tickets: {0} / {1}", wrestlingEvent.ticketsSold.ToString(), wrestlingEvent.EventVenue.capacity) : "Tickets: <TBD>");
		revenue.text = string.Format("${0}", wrestlingEvent.revenue);
	}
}
