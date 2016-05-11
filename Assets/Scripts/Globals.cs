using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

	private static Globals _instance;
	public static Globals Instance { get { return _instance; } }

	public void Awake() {
		if (_instance) Destroy(this);
		else _instance = this;
	}

	private bool gamePaused = false;
	public bool GamePaused {
		get { return gamePaused; }
		set {
			gamePaused = value;
			Time.timeScale = gamePaused ? 0 : 1;
		}
	}
}
