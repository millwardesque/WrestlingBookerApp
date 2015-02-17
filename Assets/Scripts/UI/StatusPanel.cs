using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusPanel : MonoBehaviour {
	public Text eventName;
	public Text ticketsSoldCount;
	public Slider ticketsSoldProgress;
	public Text revenue;

	public void UpdateEventStatus(WrestlingEvent wrestlingEvent) {
		eventName.text = wrestlingEvent.eventName;
		ticketsSoldCount.text = (wrestlingEvent.ticketsSold >= 0 && wrestlingEvent.EventVenue != null ? string.Format ("{0} / {1}", wrestlingEvent.ticketsSold.ToString(), wrestlingEvent.EventVenue.capacity) : "0");
		revenue.text = string.Format("${0}", wrestlingEvent.revenue);

		if (wrestlingEvent.EventVenue != null) {
			ticketsSoldProgress.minValue = 0;
			ticketsSoldProgress.maxValue = wrestlingEvent.EventVenue.capacity;
			ticketsSoldProgress.value = wrestlingEvent.ticketsSold;
		}
		else {
			ticketsSoldProgress.value = 0;
		}
	}
}
