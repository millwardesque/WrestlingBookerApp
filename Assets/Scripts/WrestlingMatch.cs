using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WrestlingTeam {
	public string name;
	public List<Wrestler> wrestlers;

	public WrestlingTeam(Wrestler wrestler) {
		this.name = wrestler.wrestlerName;
		this.wrestlers = new List<Wrestler>();
		this.wrestlers.Add (wrestler);
	}

	public WrestlingTeam(string name, List<Wrestler> wrestlers) {
		this.name = name;
		this.wrestlers = wrestlers;
	}

	public override string ToString() {
		return this.name;
	}
}

public class WrestlingMatch {
	public List<WrestlingTeam> teams = new List<WrestlingTeam>();
	public WrestlingMatchType type;
	public WrestlingMatchFinish finish;

	void Initialize(List<WrestlingTeam> teams, WrestlingMatchType type, WrestlingMatchFinish finish) {
		this.teams = teams;
		this.type = type;
		this.finish = finish;
	}

	public int ParticipantCount {
		get {
			int count = 0;
			foreach (WrestlingTeam team in teams) {
				count += team.wrestlers.Count;
			}

			return count;
		}
	}
}
