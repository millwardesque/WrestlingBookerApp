using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WrestlerManager : MonoBehaviour {
	List<Wrestler> wrestlers;
	public Wrestler wrestlerPrefab;
	
	// Use this for initialization
	void Start () {
		if (!wrestlerPrefab) {
			Debug.LogError("Unable to start Wrestler Manager: No wrestler prefab is set.");
		}
		
		wrestlers = new List<Wrestler>();
		
		// @TODO Load these from an external source.
		
		CreateWrestler("Bret Hart", "The best there is, the best there was, and the best there ever will be.");
		CreateWrestler("Ric Flair", "The Nature Boy! Whoooo!");
	}
	
	public List<Wrestler> GetWrestlers() {
		return wrestlers;
	}
	
	public Wrestler CreateWrestler(string name, string description) {
		Wrestler wrestler = Instantiate(wrestlerPrefab) as Wrestler;
		wrestler.transform.SetParent(transform, false);
		wrestler.Initialize(name, description);
		wrestlers.Add (wrestler);
		return wrestler;
	}
}
