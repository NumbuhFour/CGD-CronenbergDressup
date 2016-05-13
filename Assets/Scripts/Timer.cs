using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	[SerializeField]
	private float gametime = 90;

	[SerializeField]
	private IngameUI ui;

	[SerializeField]
	private GameObject gameoverUI;

	[SerializeField]
	private World world;

	// Use this for initialization
	void Start () {
		ui.SetMaxTime (gametime);
	}
	
	// Update is called once per frame
	void Update () {
		gametime -= Time.deltaTime;

		if (gametime <= 0) {
			gametime = 0;
			Globals.Instance.GamePaused = true;
			gameoverUI.SetActive (true);
			ui.gameObject.SetActive (true);
		}

		ui.SetTimeRemaining (gametime);

		ui.SetScore (world.Score);
	}
}
