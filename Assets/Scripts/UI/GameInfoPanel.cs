using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameInfoPanel : MonoBehaviour {
	public Text companyName;
	public Text companyMoney;
	public Text gameDate;

	public void UpdateCompanyStatus(Company company) {
		this.companyName.text = company.companyName;
		this.companyMoney.text = string.Format ("${0}", Mathf.Round(company.money));
	}

	public void UpdateTime(TimeManager timeManager) {
		this.gameDate.text = timeManager.ToString();
	}
}
