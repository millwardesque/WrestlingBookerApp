using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class TrainWrestlersState : GameState {
	PagedSelectOptionDialog wrestlerDialog;
	SelectOptionDialog trainingTypeDialog;
	InfoDialog trainAnotherDialog;

	List<Wrestler> roster;
	Wrestler selectedWrestler;

	public override void OnEnter (GameManager gameManager) {
		ChooseWrestler();
	}

	void ChooseWrestler() {
		wrestlerDialog = GameManager.Instance.GetGUIManager().InstantiatePagedSelectOptionDialog();
		wrestlerDialog.Initialize("Train a wrestler", GetWrestlersToTrain(), new UnityAction(OnChooseWrestler), true, new UnityAction(DoneTraining));
	}

	void OnChooseWrestler() {
		selectedWrestler = roster.Find ( x => x.wrestlerName == wrestlerDialog.GetSelectedOption().name );
		trainingTypeDialog = GameManager.Instance.GetGUIManager ().InstantiateSelectOptionDialog(true);
		trainingTypeDialog.Initialize("Choose a training type", GetTrainingTypes(), new UnityAction(OnTrainingChosen), true, new UnityAction(ChooseWrestler), "OK", "Back");
	}

	void OnTrainingChosen() {
		trainAnotherDialog = GameManager.Instance.GetGUIManager().InstantiateInfoDialog();
		trainAnotherDialog.Initialize("Wrestler trained!", "You trained " + selectedWrestler.wrestlerName + "!\nWould you like to train another wrestler?", new UnityAction(ChooseWrestler), true, new UnityAction(DoneTraining), "Yes", "No");
	}

	void DoneTraining() {
		ExecuteTransition ("FINISHED");
	}

	List<SelectOptionDialogOption> GetWrestlersToTrain() {
		List<SelectOptionDialogOption> wrestlerOptions = new List<SelectOptionDialogOption>();
		roster = GameManager.Instance.GetPlayerCompany().GetRoster ();
		foreach (Wrestler wrestler in roster) {
			wrestlerOptions.Add(new SelectOptionDialogOption(wrestler.wrestlerName, "", wrestler.DescriptionWithStats));
		}
		
		return wrestlerOptions;
	}

	List<SelectOptionDialogOption> GetTrainingTypes() {
		List<SelectOptionDialogOption> trainingOptions = new List<SelectOptionDialogOption>();

		trainingOptions.Add(new SelectOptionDialogOption("Study tapes", "$1000", "Work+\nCharisma+\n\nStudy tapes of old matches to see how the masters did it"));
		trainingOptions.Add(new SelectOptionDialogOption("Vocal coaching", "$2500", "Charisma++\n\nGet better on the mic"));

		return trainingOptions;
	}
}
