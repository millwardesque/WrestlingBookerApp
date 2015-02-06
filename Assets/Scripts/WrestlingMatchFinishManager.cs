using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WrestlingMatchFinishManager : MonoBehaviour {
	List<WrestlingMatchFinish> matchFinishes;
	
	// Use this for initialization
	void Start () {
		matchFinishes = new List<WrestlingMatchFinish>();
		
		// @TODO Load these from an external source.
		
		CreateWrestlingMatchFinish("Clean win", "There is a single, clear victor");
		CreateWrestlingMatchFinish("Dirty win", "The victor wins by cheating.");
	}
	
	public List<WrestlingMatchFinish> GetMatchFinishes() {
		return matchFinishes;
	}
	
	public WrestlingMatchFinish CreateWrestlingMatchFinish(string name, string description) {
		WrestlingMatchFinish matchFinish = new WrestlingMatchFinish();
		matchFinish.Initialize(name, description);
		matchFinishes.Add (matchFinish);
		return matchFinish;
	}
}
