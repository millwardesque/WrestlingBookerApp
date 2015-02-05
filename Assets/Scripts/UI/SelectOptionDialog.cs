using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class SelectOptionDialogOption {
	public string name;
	public string description;
	public bool isInteractable;
	
	public SelectOptionDialogOption(string name, string description, bool isInteractable = true) {
		this.name = name;
		this.description = description;
		this.isInteractable = isInteractable;
	}
}

public class SelectOptionDialog : MonoBehaviour {
	public Text title;
	public Button okButton;
	public Button cancelButton;
	public GameObject optionContainer;
	public Scrollbar optionScrollbar;
	public Text secondaryTitle;
	public Text secondaryDescription;
	public GameObject optionPrefab;

	List<Toggle> optionObjects = new List<Toggle>();
	List<SelectOptionDialogOption> optionData = new List<SelectOptionDialogOption>();

	int selectedIndex = -1;

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

		if (optionContainer == null) {
			LogStartError("Options container isn't set");
		}

		if (optionScrollbar == null) {
			LogStartError("Options scrollbar isn't set");
		}

		if (secondaryTitle == null) {
			LogStartError("Secondary title isn't set");
		}

		if (secondaryDescription == null) {
			LogStartError("Secondary description isn't set");
		}

		if (optionPrefab == null) {
			LogStartError("Option prefab isn't set");
		}
	}

	void LogStartError(string message) {
		Debug.LogError("Unable to start Select Option Dialog: " + message);
	}

	public void Initialize(string title, List<SelectOptionDialogOption> options, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null) {
		this.title.text = title;

		// Add the options to the toggle list.
		bool isFirst = true;
		foreach (SelectOptionDialogOption option in options) {
			GameObject optionObj = Instantiate(optionPrefab) as GameObject;
			optionObj.transform.SetParent(optionContainer.transform, false);

			Toggle optionToggle = optionObj.GetComponent<Toggle>();
			optionToggle.GetComponentInChildren<Text>().text = option.name;
			optionToggle.group = optionContainer.GetComponent<ToggleGroup>();
			optionToggle.onValueChanged.AddListener(UpdateSelected);
			optionToggle.interactable = option.isInteractable;

			// Save the data and the UI control for later usage.
			optionObjects.Add(optionToggle);
			optionData.Add (option);

			// Enable the first valid option by default.
			if (isFirst && optionToggle.interactable) {
				optionToggle.isOn = true;
			}
		}

		// Hide the scrollbar if there's no scrolling to be done.
		if (optionScrollbar.size == 1.0f) {
			optionScrollbar.gameObject.SetActive(false);
		}

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
		if (selectedIndex == -1) {
			return null;
		}
		else {
			return optionData[selectedIndex];
		}
	}

	void UpdateSelected(bool isOn) {
		for (int i = 0; i < optionObjects.Count; ++i) {
			if (optionObjects[i].isOn) {
				// Populate secondary panel.
				selectedIndex = i;
				secondaryTitle.text = optionData[i].name;
				secondaryDescription.text = optionData[i].description;
			}
		}
	}

	void ClosePanel() {
		Destroy (gameObject);
	}
}
