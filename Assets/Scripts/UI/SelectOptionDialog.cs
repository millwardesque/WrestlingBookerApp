using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class SelectOptionDialog : MonoBehaviour {
	public Text title;
	public Button okButton;
	public Button cancelButton;
	public SelectOptionControl optionControl;

	void Start() {
		if (title == null) {
			LogStartError("Title isn't set");
		}

		if (okButton == null) {
			LogStartError("OK button isn't set");
		}

		if (cancelButton == null) {
			LogStartError("Cancel button isn't set");
		}

		if (optionControl == null) {
			LogStartError("Options control isn't set");
		}
	}

	void LogStartError(string message) {
		Debug.LogError("Unable to start Select Option Dialog: " + message);
	}

	void OnGUI() {
		// Allow the Enter key to submit this form.
		if (okButton.interactable && Input.GetKey(KeyCode.Return)) {
			okButton.Select();
		}
	}

	public void Initialize(string title, List<SelectOptionDialogOption> options, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null) {
		this.title.text = title;

		optionControl.Initialize(options);

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

	public SelectOptionDialogOption GetSelectedOption() {
		return optionControl.GetSelectedOption();
	}

	void ClosePanel() {
		Destroy (gameObject);
	}
}
