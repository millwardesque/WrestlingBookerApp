using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class SinglesMatchDialog : UIDialog {
	public SelectOptionControl wrestler1Control;
	public SelectOptionControl wrestler2Control;
	
	void Start() {
		if (wrestler1Control == null) {
			LogStartError("Wrestler 1 select control isn't set");
		}

		if (wrestler2Control == null) {
			LogStartError("Wrestler 2 select control isn't set");
		}
	}
	
	public void Initialize(string title, List<SelectOptionDialogOption> wrestler1Options, List<SelectOptionDialogOption> wrestler2Options, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null) {
		base.Initialize(title, okAction, canCancel, cancelAction);
		
		wrestler1Control.Initialize(wrestler1Options, 0);
		wrestler2Control.Initialize(wrestler2Options, 1);
		OnWrestler1Changed(true); // Manually trigger the functionality to make sure wrestler 2 can't be set to wrestler 1
		OnWrestler2Changed(true); // Manually trigger the functionality to make sure wrestler 1 can't be set to wrestler 2

		wrestler1Control.AddChangeListener(new UnityAction<bool>(OnWrestler1Changed));
		wrestler2Control.AddChangeListener(new UnityAction<bool>(OnWrestler2Changed));
	}

	public void OnWrestler1Changed(bool isOn) {
		// Make sure wrestler 2 can't select the same wrestler.
		if (isOn) {
			wrestler2Control.DisableOption(GetWrestler1().name, true);
		}
	}

	public void OnWrestler2Changed(bool isOn) {
		// Make sure wrestler 1 can't select the same wrestler.
		if (isOn) {
			wrestler1Control.DisableOption(GetWrestler2().name, true);
		}
	}
	
	public SelectOptionDialogOption GetWrestler1() {
		return wrestler1Control.GetSelectedOption();
	}

	public SelectOptionDialogOption GetWrestler2() {
		return wrestler2Control.GetSelectedOption();
	}
}
