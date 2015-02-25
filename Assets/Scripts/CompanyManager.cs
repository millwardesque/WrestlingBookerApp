using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class CompanyManager : MonoBehaviour {
	List<Company> companies = new List<Company>();
	Company companyPrefab = null;
	
	void Awake() {
		companyPrefab = Resources.Load<Company>("Company");
		if (companyPrefab == null) {
			Debug.LogError ("Unable to start company manager: No Company named 'Company' is available in the Resources folder.");
		}

		LoadFromJSON("companies");
	}

	void LoadFromJSON(string filename) {
		TextAsset jsonAsset = Resources.Load<TextAsset>(filename);
		if (jsonAsset != null) {
			string fileContents = jsonAsset.text;
			var N = JSON.Parse(fileContents);
			var companyArray = N["companies"].AsArray;
			foreach (JSONNode company in companyArray) {
				string name = company["name"];
				float money = company["money"].AsFloat;
				int maxRosterSize = company["maxRosterSize"].AsInt;
				int phase = company["phase"].AsInt;
				bool isInAlliance = company["isInAlliance"].AsBool;
				List<Wrestler> roster = new List<Wrestler>();

				CreateCompany (name, money, maxRosterSize, phase, roster, isInAlliance);
			}
		}
		else {
			Debug.LogError("Unable to load event type data from JSON at '" + filename + "': There was an error opening the file.");
		}
	}

	public List<Company> GetCompanies(int phase) {
		return companies.FindAll( x => x.phase <= phase );
	}

	public Company CreateCompany() {
		return GameObject.Instantiate(companyPrefab) as Company;
	}

	public Company CreateCompany(string name, float money, int maxRosterSize, int phase, List<Wrestler> roster, bool isInAlliance) {
		Company newCompany = GameObject.Instantiate(companyPrefab) as Company;
		newCompany.transform.SetParent(transform, false);
		newCompany.Initialize(name, money, maxRosterSize, phase, roster, isInAlliance);
		newCompany.name = newCompany.companyName;
		return newCompany;
	}
}
