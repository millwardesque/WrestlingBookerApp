using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class SelectOptionDialog : UIDialog {
	public SelectOptionControl optionControl;

	void Start() {
		if (optionControl == null) {
			LogStartError("Options control isn't set");
		}
	}

	public void Initialize(string title, List<SelectOptionDialogOption> options, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null, string okLabel = "OK", string cancelLabel = "Cancel") {
		base.Initialize(title, okAction, canCancel, cancelAction, okLabel, cancelLabel);
		optionControl.Initialize(options);
	}

	public SelectOptionDialogOption GetSelectedOption() {
		return optionControl.GetSelectedOption();
	}
}
