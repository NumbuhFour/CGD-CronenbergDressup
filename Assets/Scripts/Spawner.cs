using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	[SerializeField]
	private GameObject[] prefabs;
	[SerializeField]
	private GameObject container;

	[SerializeField]
	private float spawnRange = 5f;

	[SerializeField]
	private float spawnChance = 0.01f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range (0.0f, 1.0f) < spawnChance) {
			Spawn ();
		}
	}

	void Spawn() {
		GameObject go = Instantiate (prefabs [Random.Range (0, prefabs.Length)]);
		go.SetActive (true);
		go.transform.position = this.transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0, 0);
		go.transform.rotation.eulerAngles.Set(0, 0, Random.Range(0f,360f));
		go.GetComponent<Rigidbody2D> ().AddTorque (Random.Range (0f, 10f));
		go.transform.SetParent (container.transform, true);
	}
}
