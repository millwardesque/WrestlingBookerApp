using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TrainingEffect {
	public string attribute;
	public float minValue;
	public float maxValue;
	public TrainingEffect(string attribute, float minValue, float maxValue) {
		this.attribute = attribute;
		this.minValue = minValue;
		this.maxValue = maxValue;
	}
}

public class Training {
	public string trainingName;
	public string description;
	public float cost;
	public int phase;
	public List<TrainingEffect> effects;

	public void Initialize(string name, string description, float cost, List<TrainingEffect> effects, int phase) {
		trainingName = name;
		this.description = description;
		this.cost = cost;
		this.effects = effects;
		this.phase = phase;
	}

	public void ApplyEffects(Company company, Wrestler wrestler) {
		company.money -= cost;
		foreach (TrainingEffect effect in effects) {
			switch (effect.attribute) {
			case "charisma":
				wrestler.charisma += Random.Range(effect.minValue, effect.maxValue);
				break;
			case "work":
				wrestler.work += Random.Range(effect.minValue, effect.maxValue);
				break;
			case "appearance":
				wrestler.appearance += Random.Range(effect.minValue, effect.maxValue);
				break;
			default:
				break;
			}
		}
	}

	public override string ToString() {
		return string.Format("${0} : Ph {1} E: {2}", cost, phase, effects.Count);
	}
}
