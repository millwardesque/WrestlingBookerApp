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

	public string GetCompanyFilename(string gameID) {
		return gameID + ".companies";
	}

	public void Save(string gameID) {
		foreach (Company company in companies) {
			Company.Save(company, gameID);
		}
	}

	public void Load(string gameID) {
		DestroyCurrentGameObjects();

		string companyFilename = GetCompanyFilename(gameID);
		if (ES2.Exists(companyFilename)) {
			string[] tags = ES2.GetTags(companyFilename);
			foreach (string tag in tags) {
				Company company = Instantiate(companyPrefab) as Company;
				company.transform.SetParent(transform, false);
				Company.Load (company, tag, gameID);
				companies.Add (company);
			}
		}
	}

	public void DeleteSaved(string gameID) {
		string companyLocation = GetCompanyFilename(gameID);
		if (ES2.Exists(companyLocation)) {
			ES2.Delete(companyLocation);
		}

		if (gameID == SavedGameManager.Instance.CurrentGameID) {
			DestroyCurrentGameObjects();
		}
	}

	public void CreateNew() {
		int[] phaseCounts = new int[4];
		phaseCounts[0] = 2;
		phaseCounts[1] = 4;
		phaseCounts[2] = 2;
		phaseCounts[3] = 2;
		
		DestroyCurrentGameObjects();

		companyGenerator.Initialize();
		for (int phase = 0; phase < phaseCounts.Length; ++phase) {
			for (int i = 0; i < phaseCounts[phase]; ++i) {
				GenerateNewCompany(phase);
			}
		}
	}

	void DestroyCurrentGameObjects() {
		foreach (Company company in companies) {
			Destroy(company.gameObject);
		}
		companies.Clear();
	}

	public void AddCompany(Company company) {
		companies.Add (company);
	}

	public Company GetCompany(string companyID) {
		return companies.Find( x => x.id == companyID );
	}
	public List<Company> GetCompanies(int phase) {
		return companies.FindAll( x => x.phase <= phase );
	}

	public Company CreateEmptyCompany() {
		Company newCompany = GameObject.Instantiate(companyPrefab) as Company;
		newCompany.transform.SetParent(transform, false);
		return newCompany;
	}

	void GenerateNewCompany(int phase) {
		Company newCompany = CreateEmptyCompany();
		companyGenerator.Generate(newCompany, phase);
		companies.Add(newCompany);
	}
}
