using UnityEngine;
using System.Collections;

public class WrestlingMatchFinish {
	public string finishName;
	public string description;
	public int phase;

	public void Initialize(string finishName, string description, int phase) {
		this.finishName = finishName;
		this.description = description;
		this.phase = phase;
	}
}
