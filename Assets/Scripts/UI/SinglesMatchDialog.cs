using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class SinglesMatchDialog : MonoBehaviour {
	public Text title;
	public Button okButton;
	public Button cancelButton;
	public SelectOptionControl wrestler1Control;
	public SelectOptionControl wrestler2Control;
	
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
		
		if (wrestler1Control == null) {
			LogStartError("Wrestler 1 select control isn't set");
		}

		if (wrestler2Control == null) {
			LogStartError("Wrestler 2 select control isn't set");
		}
	}
	
	void LogStartError(string message) {
		Debug.LogError("Unable to start Singles Match Dialog: " + message);
	}
	
	public void Initialize(string title, List<SelectOptionDialogOption> wrestler1Options, List<SelectOptionDialogOption> wrestler2Options, UnityAction okAction, bool canCancel = false, UnityAction cancelAction = null) {
		this.title.text = title;
		
		wrestler1Control.Initialize(wrestler1Options);
		wrestler2Control.Initialize(wrestler2Options);
		
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
	
	public SelectOptionDialogOption GetWrestler1() {
		return wrestler1Control.GetSelectedOption();
	}

	public SelectOptionDialogOption GetWrestler2() {
		return wrestler2Control.GetSelectedOption();
	}
	
	void ClosePanel() {
		Destroy (gameObject);
	}
}
