using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;
public class Restart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onRestart() {
		SceneManager.LoadScene(1);
	}

	public void onMenu() {
		SceneManager.LoadScene (0);
	}
}
