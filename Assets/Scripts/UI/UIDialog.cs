﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class UIDialog : MonoBehaviour {
	public Text title;
	public Button okButton;
	public Button cancelButton;

	// Use this for initialization
	void Start () {
		if (title == null) {
			LogStartError("Title isn't set");
		}
		
		if (okButton == null) {
			LogStartError("OK button isn't set");
		}
		
		if (cancelButton == null) {
			LogStartError("Cancel button isn't set");
		}
	}

	protected void Initialize(string title, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null, string okLabel = "OK", string cancelLabel = "Cancel") {
		SetTitle(title);

		okButton.GetComponentInChildren<Text>().text = okLabel;
		cancelButton.GetComponentInChildren<Text>().text = cancelLabel;
		
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

	protected void SetTitle(string title) {
		this.title.text = title;
	}

	void OnGUI() {
		// Allow the Enter key to submit this form.
		if (okButton.interactable && Input.GetKey(KeyCode.Return)) {
			okButton.Select();
		}
	}

	protected void LogStartError(string message) {
		Debug.LogError("Unable to start '" + name + "' dialog: " + message);
	}

	protected void ClosePanel() {
		Destroy (gameObject);
	}

	protected void ShowOKButton() {
		okButton.gameObject.SetActive(true);
	}

	protected void HideOKButton() {
		okButton.gameObject.SetActive(false);
	}
}
