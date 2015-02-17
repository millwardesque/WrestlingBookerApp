using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationPanel : MonoBehaviour {
	public int maxVisibleNotifications;
	public float notificationLifetime;
	public UINotification notificationPrefab;
	Queue<UINotification> notifications = new Queue<UINotification>();
	List<UINotification> visibleNotifications = new List<UINotification>();

	void Awake() {
		if (notificationPrefab == null) {
			Debug.LogError("Unable to wake notification panel: No notification prefab is set.");
		}
	}

	public void QueueNotification(string message) {
		UINotification newNotification = GameObject.Instantiate(notificationPrefab) as UINotification;
		newNotification.gameObject.SetActive(false);
		newNotification.Initialize(message, notificationLifetime);
		notifications.Enqueue(newNotification);
	}

	void Update() {
		// Clean up dead notifications.
		List<UINotification> deadList = new List<UINotification>();
		foreach (UINotification notification in visibleNotifications) {
			if (notification == null) {
				deadList.Add(notification);
			}
		}
		foreach (UINotification notification in deadList) {
			visibleNotifications.Remove(notification);
		}

		// Add queued notifications if there's room.
		while (notifications.Count > 0 && visibleNotifications.Count < maxVisibleNotifications) {
			UINotification notification = notifications.Dequeue();
			notification.transform.SetParent(transform, false);
			notification.Show();
			visibleNotifications.Add (notification);
		}
	}
}
