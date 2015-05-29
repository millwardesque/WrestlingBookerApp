using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WrestlingMatchFinish {
	public string finishName;
	public string description;
	public int phase;
	List<WrestlingMatchType> incompatibleMatchTypes = new List<WrestlingMatchType>();

	public void Initialize(string finishName, string description, int phase, List<WrestlingMatchType> incompatibleMatchTypes) {
		this.finishName = finishName;
		this.description = description;
		this.phase = phase;
		this.incompatibleMatchTypes = incompatibleMatchTypes;
	}

	public bool IsCompatibleWithMatchType(WrestlingMatchType matchType) {
		return (incompatibleMatchTypes.Find( x => x.typeName == matchType.typeName ) == null);
	}
}
