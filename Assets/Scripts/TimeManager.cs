using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {
	float fractional = 0.0f;
	int week;
	int month;
	int year;
	public float weekLength = 1.0f;
	GameManager gameManager;

	// Use this for initialization
	void Start () {
		fractional = 0.0f;
		week = 0;
		month = 0;
		year = 0;

		gameManager = GameObject.FindObjectOfType<GameManager>();
		if (gameManager == null) {
			Debug.LogError ("Unable to start Time manager: No Game manager was found with the GameManager script.");
		}

		// Force the game to acknowledge the starting time.
		gameManager.OnTimeUpdated(this);
	}
	
	// Update is called once per frame
	void Update () {
		fractional += Time.deltaTime * weekLength;
		ProcessTimeOverflow();	
	}

	void ProcessTimeOverflow() {
		while (Mathf.FloorToInt(fractional) >= 1.0f) {
			fractional -= 1.0f;
			week++;

			if (week >= 4) {
				week -= 4;
				month++;

				if (month >= 12) {
					month -= 12;
					year++;
				}
			}

			gameManager.OnTimeUpdated(this);
		}
	}

	public override string ToString() {
		// Add one to each of the values to remove the confusion of 0-based numbering.
		return string.Format("Y{0} : M{1} : W{2}", year + 1, month + 1, week + 1);
	}
}
