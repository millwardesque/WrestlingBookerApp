using UnityEngine;
using System.Collections;

public class WrestlingMatchFinish {
	public string finishName;
	public string description;

	public void Initialize(string finishName, string description) {
		this.finishName = finishName;
		this.description = description;
	}
}
