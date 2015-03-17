using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompanyManager : MonoBehaviour {
	List<Company> companies = new List<Company>();
	Company companyPrefab = null;
	CompanyGenerator companyGenerator = new CompanyGenerator();

	public static CompanyManager Instance;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
			companyPrefab = Resources.Load<Company>("Company");

			if (companyPrefab == null) {
				Debug.LogError ("Unable to start company manager: No Company named 'Company' is available in the Resources folder.");
			}
		}
		else {
			Destroy (gameObject);
		}
	}

	public void LoadCompanies() {
		if (!LoadSavedCompanies()) {
			GenerateNewCompanies();
		}
	}

	public string CompanyFilename {
		get { return GameManager.Instance.GameID + ".companies"; }
	}

	bool LoadSavedCompanies() {
		if (ES2.Exists(CompanyFilename)) {
			string[] tags = ES2.GetTags(CompanyFilename);
			foreach (string tag in tags) {
				string companyLocation = CompanyFilename + "?tag=" + tag;
				Company company = Instantiate(companyPrefab) as Company;
				company.transform.SetParent(transform, false);
				ES2.Load<Company>(companyLocation, company);
				companies.Add (company);
			}
			return true;
		}
		else {
			return false;
		}
	}

	public void GenerateNewCompanies() {
		int[] phaseCounts = new int[4];
		phaseCounts[0] = 4;
		phaseCounts[1] = 2;
		phaseCounts[2] = 2;
		phaseCounts[3] = 1;
		
		// Clean up existing data.
		foreach (Company company in companies) {
			Destroy(company.gameObject);
		}
		companies.Clear();
		
		if (ES2.Exists(CompanyFilename)) {
			ES2.Delete (CompanyFilename);
		}
		
		companyGenerator.Initialize();
		
		for (int phase = 0; phase < phaseCounts.Length; ++phase) {
			for (int i = 0; i < phaseCounts[phase]; ++i) {
				GenerateNewCompany(phase);
			}
		}
	}

	public List<Company> GetCompanies(int phase) {
		return companies.FindAll( x => x.phase <= phase );
	}

	public void ClearSavedData() {
		GenerateNewCompanies();
	}

	public Company CreateCompany() {
		return GameObject.Instantiate(companyPrefab) as Company;
	}

	public void GenerateNewCompany(int phase) {
		Company newCompany = GameObject.Instantiate(companyPrefab) as Company;
		newCompany.transform.SetParent(transform, false);

		newCompany.Save ();
		companies.Add(newCompany);
	}
}
