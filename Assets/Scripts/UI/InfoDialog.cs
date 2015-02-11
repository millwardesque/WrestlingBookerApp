using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class InfoDialog : UIDialog {
	public Text message;

	void Start() {
		if (message == null) {
			LogStartError("Message isn't set");
		}
	}

	public void Initialize(string title, string message, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null) {
		this.message.text = message;
		base.Initialize(title, okAction, canCancel, cancelAction);
	}
}
