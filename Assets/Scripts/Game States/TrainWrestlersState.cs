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

	List<Training> trainingOptions;
	Training selectedTraining;

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
		selectedTraining = trainingOptions.Find( x => x.trainingName == trainingTypeDialog.GetSelectedOption().name );
		selectedTraining.ApplyEffects(GameManager.Instance.GetPlayerCompany(), selectedWrestler);
		GameManager.Instance.OnCompanyUpdated();

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
		List<SelectOptionDialogOption> trainingTypeOptions = new List<SelectOptionDialogOption>();

		trainingOptions = TrainingManager.Instance.GetTrainingOptions(GameManager.Instance.GetPhase());
		foreach (Training training in trainingOptions) {
			bool canUse = true;
			if (training.cost > GameManager.Instance.GetPlayerCompany().money) {
				canUse = false;
			}
			trainingTypeOptions.Add(new SelectOptionDialogOption(training.trainingName, string.Format("${0}", training.cost), training.description, canUse));
		}

		return trainingTypeOptions;
	}
}
