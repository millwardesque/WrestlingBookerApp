using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PagedSelectOptionDialog : UIDialog {
	List<SelectOptionDialogOption> options;
	int currentOption = 0;
	public Text selectedName;
	public Text selectedDescription;
	string baseTitle;

	void Awake() {
		if (selectedName == null) {
			Debug.LogError("Unable to start Paged Selected Option Dialog: No selected-name text is set.");
		}

		if (selectedDescription == null) {
			Debug.LogError("Unable to start Paged Selected Option Dialog: No selected-description text is set.");
		}
	}

	public void Initialize(string title, List<SelectOptionDialogOption> options, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null, string okLabel = "OK", string cancelLabel = "Cancel") {
		baseTitle = title;
		base.Initialize(baseTitle, okAction, canCancel, cancelAction, okLabel, cancelLabel);
		this.options = options;
		RefreshDisplayedOption();
	}

	public void OnNextClick() {
		currentOption = (currentOption + 1) % options.Count;
		RefreshDisplayedOption();
	}

	public void OnPreviousClick() {
		currentOption = (options.Count + currentOption - 1) % options.Count;
		RefreshDisplayedOption();
	}

	public SelectOptionDialogOption GetSelectedOption() {
		return options[currentOption];
	}

	void RefreshDisplayedOption() {
		SelectOptionDialogOption selected = GetSelectedOption();
		selectedName.text = selected.name;
		selectedDescription.text = selected.description;
		SetTitle(string.Format ("{0} {1}/{2}", baseTitle, (currentOption + 1), options.Count));
	}
}
