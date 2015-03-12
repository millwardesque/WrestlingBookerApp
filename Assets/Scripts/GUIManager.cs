using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {
	public Canvas canvas;
	public GameObject selectOptionDialogPrefab;
	public GameObject wideSelectOptionDialogPrefab;
	public GameObject pagedSelectOptionDialogPrefab;
	public GameObject textInputDialogPrefab;
	public GameObject infoDialogPrefab;
	public GameObject singlesMatchDialogPrefab;
	public Slider progressBarPrefab;
	public StatusPanel statusPanel;
	public GameInfoPanel gameInfoPanel;
	public NotificationPanel notificationPanel;
	public GameObject menuPanel;

	SelectOptionDialog dialogBox;
	TextInputDialog textInputDialogBox;
	GameManager gameManager;

	void Awake() {
		if (canvas == null) {
			Debug.LogError("Unable to start GUI Manager: Canvas isn't set.");
		}

		if (statusPanel == null) {
			Debug.LogError("Unable to start GUI Manager: Status panel isn't set");
		}

		if (gameInfoPanel == null) {
			Debug.LogError("Unable to start GUI Manager: Game Info panel isn't set");
		}

		if (notificationPanel == null) {
			Debug.LogError("Unable to start GUI Manager: Notification panel isn't set");
		}

		if (menuPanel == null) {
			Debug.LogError("Unable to start GUI Manager: Menu panel isn't set");
		}

		if (selectOptionDialogPrefab == null || selectOptionDialogPrefab.GetComponent<SelectOptionDialog>() == null) {
			Debug.LogError("Unable to start GUI Manager: Select Option Dialog prefab isn't set or is missing SelectOptionDialog script.");
		}

		if (wideSelectOptionDialogPrefab == null || wideSelectOptionDialogPrefab.GetComponent<SelectOptionDialog>() == null) {
			Debug.LogError("Unable to start GUI Manager: Wide Select Option Dialog prefab isn't set or is missing SelectOptionDialog script.");
		}

		if (pagedSelectOptionDialogPrefab == null || pagedSelectOptionDialogPrefab.GetComponent<PagedSelectOptionDialog>() == null) {
			Debug.LogError("Unable to start GUI Manager: Paged Select Option Dialog prefab isn't set or is missing PagedSelectOptionDialog script.");
		}

		if (textInputDialogPrefab == null || textInputDialogPrefab.GetComponent<TextInputDialog>() == null) {
			Debug.LogError("Unable to start GUI Manager: Text Input Dialog prefab isn't set or is missing TextInputDialog script.");
		}

		if (infoDialogPrefab == null || infoDialogPrefab.GetComponent<InfoDialog>() == null) {
			Debug.LogError("Unable to start GUI Manager: Info Dialog prefab isn't set or is missing InfoDialog script.");
		}

		if (singlesMatchDialogPrefab == null || singlesMatchDialogPrefab.GetComponent<SinglesMatchDialog>() == null) {
			Debug.LogError("Unable to start GUI Manager: Singles Match Dialog prefab isn't set or is missing SinglesMatchDialog script.");
		}

		if (progressBarPrefab == null) {
			Debug.LogError("Unable to start GUI Manager: Progress bar prefab isn't set.");
		}

		HideMenu();
	}

	void Start() {
		GameObject gameManagerObj = GameObject.FindGameObjectWithTag("Game Manager");
		if (gameManagerObj == null || gameManagerObj.GetComponent<GameManager>() == null) {
			Debug.LogError("Unable to start\t GUI Manager: No object in the scene is tagged Game Manager.");
		}
		gameManager = gameManagerObj.GetComponent<GameManager>();
	}

	public SelectOptionDialog InstantiateSelectOptionDialog(bool useWideVersion = false) {
		GameObject dialogObj;

		if (useWideVersion) {
			dialogObj = Instantiate(wideSelectOptionDialogPrefab) as GameObject;
		}
		else {
			dialogObj = Instantiate(selectOptionDialogPrefab) as GameObject;
		}
		dialogObj.transform.SetParent(canvas.transform, false);
		
		return dialogObj.GetComponent<SelectOptionDialog>();
	}

	public PagedSelectOptionDialog InstantiatePagedSelectOptionDialog() {
		GameObject dialogObj = Instantiate(pagedSelectOptionDialogPrefab) as GameObject;
		dialogObj.transform.SetParent(canvas.transform, false);
		
		return dialogObj.GetComponent<PagedSelectOptionDialog>();
	}

	public TextInputDialog InstantiateTextInputDialog() {
		GameObject dialogObj = Instantiate(textInputDialogPrefab) as GameObject;
		dialogObj.transform.SetParent(canvas.transform, false);
		
		return dialogObj.GetComponent<TextInputDialog>();
	}

	public InfoDialog InstantiateInfoDialog() {
		GameObject dialogObj = Instantiate(infoDialogPrefab) as GameObject;
		dialogObj.transform.SetParent(canvas.transform, false);
		
		return dialogObj.GetComponent<InfoDialog>();
	}

	public SinglesMatchDialog InstantiateSinglesMatchDialog() {
		GameObject dialogObj = Instantiate(singlesMatchDialogPrefab) as GameObject;
		dialogObj.transform.SetParent(canvas.transform, false);
		
		return dialogObj.GetComponent<SinglesMatchDialog>();
	}

	public void OnCreateEventClick() {
		HideMenu();
		gameManager.CreateNewEventAttempt();
	}

	public void OnHireWrestlersClick() {
		HideMenu();
		gameManager.HireWrestlers();
	}

	public void OnFireWrestlerClick() {
		HideMenu();
		gameManager.FireWrestler();
	}

	public void OnMainMenuClick() {
		HideMenu();
		gameManager.GoToMainMenu();
	}

	public void OnClearSavedClick() {
		gameManager.ClearSavedData();
	}

	public StatusPanel GetStatusPanel() {
		return statusPanel;
	}

	public void ShowMenu() {
		menuPanel.SetActive(true);
	}

	public void HideMenu() {
		menuPanel.SetActive(false);
	}

	public void ShowStatusPanel() {
		statusPanel.gameObject.SetActive(true);
	}

	public void HideStatusPanel() {
		statusPanel.gameObject.SetActive(false);
	}

	public GameInfoPanel GetGameInfoPanel() {
		return gameInfoPanel;
	}

	public void AddNotification(string message) {
		notificationPanel.QueueNotification(message);
	}
}
