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

public class SelectOptionControl : MonoBehaviour {
	public Toggle optionPrefab;
	public GameObject optionContainer;
	public Scrollbar optionScrollbar;
	public Text secondaryTitle;
	public Text secondaryDescription;

	List<Toggle> optionObjects = new List<Toggle>();
	List<SelectOptionDialogOption> optionData = new List<SelectOptionDialogOption>();
	int selectedIndex = -1;

	void Start() {
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
		Debug.LogError("Unable to start Select Option control: " + message);
	}
	
	public void Initialize(List<SelectOptionDialogOption> options, int defaultIndex = 0) {		
		// Add the options to the toggle list.
		int count = 0;
		foreach (SelectOptionDialogOption option in options) {			
			Toggle optionToggle = Instantiate(optionPrefab) as Toggle;
			optionToggle.transform.SetParent(optionContainer.transform, false);
			optionToggle.GetComponentInChildren<Text>().text = option.name;
			optionToggle.group = optionContainer.GetComponent<ToggleGroup>();
			optionToggle.onValueChanged.AddListener(UpdateSelected);
			optionToggle.interactable = option.isInteractable;
			
			// Save the data and the UI control for later usage.
			optionObjects.Add(optionToggle);
			optionData.Add (option);
			
			// Enable the first valid option by default.
			if (count == defaultIndex && optionToggle.interactable) {
				optionToggle.isOn = true;
			}
			count++;
		}
		UpdateScrollbar();
	}

	void Update() {
		UpdateScrollbar();
	}

	void UpdateScrollbar() {
		// Show / hide the scrollbar depending on whether there's scrolling to be done.
		float myEpsilon = 0.0000001f;
		if (optionScrollbar.gameObject.activeSelf && (Mathf.Abs (1.0f - optionScrollbar.size) <= myEpsilon)) {
			optionScrollbar.gameObject.SetActive(false);
		}
		else if (!optionScrollbar.gameObject.activeSelf && (Mathf.Abs (1.0f - optionScrollbar.size) > myEpsilon)) {
			optionScrollbar.gameObject.SetActive(true);
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

	public void AddChangeListener(UnityAction<bool> listener) {
		foreach (Toggle option in optionObjects) {
			option.onValueChanged.AddListener(listener);
		}
	}

	public void DisableOption(string name, bool enableAllOthers) {
		for (int i = 0; i < optionObjects.Count; ++i) {
			if (optionData[i].name == name) {
				optionObjects[i].interactable = false;
			}
			else if (enableAllOthers) {
				optionObjects[i].interactable = true;
			}
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
}
