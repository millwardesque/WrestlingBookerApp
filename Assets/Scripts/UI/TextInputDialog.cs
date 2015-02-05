﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class TextInputDialog : MonoBehaviour {
	public Text title;
	public Text description;
	public InputField inputField;
	public Button okButton;
	public Button cancelButton;
	public int maxLength = 999;

	void Start() {
		if (title == null) {
			LogStartError("Title isn't set");
		}

		if (description == null) {
			LogStartError("Description isn't set");
		}

		if (inputField == null) {
			LogStartError("Input field isn't set");
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

	public void Initialize(string title, string description, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null) {
		this.title.text = title;
		this.description.text = description;

		// Set up the input field validation, and manually invoke once.
		inputField.onValueChange.AddListener(ValidateInput);
		ValidateInput(inputField.text);

		// Focus on the input field
		EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
		inputField.OnPointerClick(new PointerEventData(EventSystem.current));

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

	void ValidateInput(string input) {
		if (inputField.text.Length == 0) {
			okButton.interactable = false;
		}
		else if (!okButton.interactable) {
			okButton.interactable = true;
		}

		if (inputField.text.Length > maxLength) {
			inputField.text = input.Substring(0, maxLength);
		}
	}

	void ClosePanel() {
		Destroy (gameObject);
	}

	public string GetUserText() {
		return inputField.text;
	}
}
