using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour {

	[SerializeField]
	private GameObject startNode;
	[SerializeField]
	private GameObject bodyContainer;

	private List<GameObject> connections = new List<GameObject>();

	[SerializeField]
	private string layer;

	private void SetLayerRecursively(Transform t, int layer) {
		t.gameObject.layer = layer;
		for (int i = 0; i < t.childCount; i++)
			SetLayerRecursively (t.GetChild (i), layer);
	}

	public void addConnection(GameObject go) {
		go.transform.SetParent (bodyContainer.transform, true);

		SetLayerRecursively (go.transform, LayerMask.NameToLayer (layer));

		Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();
		//rb.constraints = RigidbodyConstraints2D.FreezeAll;
		Transform conn = go.transform.FindChild ("_connections");
		for (int i = 0; i < conn.childCount; i++) {
			Transform t = conn.GetChild (i);
			t.gameObject.SetActive (true);
			if (t.name == "_cnb") {
				GameObject c = t.gameObject;
				connections.Add (c);
			}
		}

		List<GameObject> rem = new List<GameObject> ();

		foreach (GameObject c in connections) {
			if (c.name != "_cnb")
				rem.Add (c);
		}
		foreach (GameObject r in rem)
			connections.Remove (r);
	}

	public List<GameObject> Connections { get { return connections; } }

	// Use this for initialization
	void Start () {
		addConnection (startNode);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
