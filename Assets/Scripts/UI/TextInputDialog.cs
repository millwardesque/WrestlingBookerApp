using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class TextInputDialog : UIDialog {
	public Text description;
	public InputField inputField;
	public int maxLength = 999;

	void Start() {
		if (description == null) {
			LogStartError("Description isn't set");
		}

		if (inputField == null) {
			LogStartError("Input field isn't set");
		}
	}

	public void Initialize(string title, string description, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null) {
		base.Initialize(title, okAction, canCancel, cancelAction);
		this.description.text = description;

		// Set up the input field validation, and manually invoke once.
		inputField.onValueChange.AddListener(ValidateInput);
		ValidateInput(inputField.text);

		// Focus on the input field
		EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
		inputField.OnPointerClick(new PointerEventData(EventSystem.current));
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

	public string GetUserText() {
		return inputField.text;
	}
}
