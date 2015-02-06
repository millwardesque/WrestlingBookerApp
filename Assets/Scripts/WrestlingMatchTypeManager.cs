using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WrestlingMatchTypeManager : MonoBehaviour {
	List<WrestlingMatchType> matchTypes;

	// Use this for initialization
	void Start () {
		matchTypes = new List<WrestlingMatchType>();
		
		// @TODO Load these from an external source.
		
		CreateWrestlingMatchType("Standard", "The standard singles match. Win by pinfall or submission.");
		CreateWrestlingMatchType("No DQ", "No disqualifications / count-outs! Win by pinfall or submission only.");
	}
	
	public List<WrestlingMatchType> GetMatchTypes() {
		return matchTypes;
	}
	
	public WrestlingMatchType CreateWrestlingMatchType(string name, string description) {
		WrestlingMatchType matchType = new WrestlingMatchType();
		matchType.Initialize(name, description);
		matchTypes.Add (matchType);
		return matchType;
	}
}
