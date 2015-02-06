using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WrestlingTeam {
	public string name;
	public List<Wrestler> wrestlers;

	public WrestlingTeam(string name, List<Wrestler> wrestlers) {
		this.name = name;
		this.wrestlers = wrestlers;
	}

	public override string ToString() {
		return this.name;
	}
}

public class WrestlingMatch {
	public List<WrestlingTeam> teams;
	public WrestlingMatchType type;
}
