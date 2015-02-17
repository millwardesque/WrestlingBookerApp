using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UINotification : MonoBehaviour {
	public float lifespan = 1.0f;
	bool isShown = false;

	public bool IsAlive {
		get { return lifespan > 0.0f; }
	}
	
	// Update is called once per frame
	void Update () {
		if (isShown) {
			if (IsAlive) {
				lifespan -= Time.deltaTime;
			}
			else if (!IsAlive) {
				GameObject.Destroy(gameObject);
			}
		}
	}

	public void Initialize(string message, float lifespan) {
		this.lifespan = lifespan;
		GetComponent<Text>().text = message;
	}

	public void Show() {
		isShown = true;
		this.gameObject.SetActive(true);
	}
}
