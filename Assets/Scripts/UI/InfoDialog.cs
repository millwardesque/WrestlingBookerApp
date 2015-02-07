using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class InfoDialog : MonoBehaviour {
	public Text title;
	public Text message;
	public Button okButton;
	public Button cancelButton;

	void Start() {
		if (title == null) {
			LogStartError("Title isn't set");
		}
		
		if (message == null) {
			LogStartError("Message isn't set");
		}
		
		if (okButton == null) {
			LogStartError("OK button isn't set");
		}
		
		if (cancelButton == null) {
			LogStartError("Cancel button isn't set");
		}
	}
	
	void LogStartError(string message) {
		Debug.LogError("Unable to start Text Input Dialog: " + message);
	}

	void OnGUI() {
		// Allow the Enter key to submit this form.
		if (okButton.interactable && Input.GetKey(KeyCode.Return)) {
			okButton.Select();
		}
	}

	public void Initialize(string title, string message, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null) {
		this.title.text = title;
		this.message.text = message;

		// Set up click handlers for the buttons.
		okButton.onClick.AddListener(ClosePanel);
		if (okAction != null) {
			okButton.onClick.AddListener(okAction);
		}
		
		if (canCancel) {
			cancelButton.onClick.AddListener(ClosePanel);
			if (cancelAction != null) {
				cancelButton.onClick.AddListener(cancelAction);
			}
		}
		else {
			Destroy(cancelButton.gameObject);
		}
	}

	void ClosePanel() {
		Destroy (gameObject);
	}
}
