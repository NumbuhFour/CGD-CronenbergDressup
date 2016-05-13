using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour {

	[SerializeField]
	private GameObject startNode;
	[SerializeField]
	private GameObject bodyContainer;

	private List<GameObject> connections = new List<GameObject>();
	private List<GameObject> clothingConnections = new List<GameObject>();

	private int score = 0;
	public int Score { get { return score; } }

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
			} else if (t.name == "_cnc") {
				clothingConnections.Add (t.gameObject);
			}
		}
		this.CleanConnections ();

	}

	public void CleanConnections() {
		List<GameObject> rem = new List<GameObject> ();
		List<GameObject> crem = new List<GameObject> ();

		foreach (GameObject c in connections) {
			if (c.name == "_cnbx")
				rem.Add (c);
			if (c.name == "_cncx")
				crem.Add (c);
		}
		foreach (GameObject r in rem)
			connections.Remove (r);
		foreach (GameObject r in crem)
			clothingConnections.Remove (r);
	}

	public List<GameObject> Connections { get { return connections; } }
	public List<GameObject> Clothes { get { return clothingConnections; } }

	// Use this for initialization
	void Start () {
		addConnection (startNode);
	}
	
	// Update is called once per frame
	void Update () {
		score = (int)CalcScore ();
	}

	public float CalcScore() {
		return CalcScore_rec(startNode.GetComponent<Connectable>(), null);
	}

	private float CalcScore_rec(Connectable conn, Clothing parentClothing) {
		Clothing myClothing = conn.Clothing;
		float myScore = GetScore(myClothing);
		bool multiply = (parentClothing != null && myClothing != null && parentClothing.setID == myClothing.setID);
		float childScore = 0.0f;
		foreach (Connectable child in conn.Children) {
			multiply |= (child.Clothing != null && myClothing != null && child.Clothing.setID == myClothing.setID);
			childScore += CalcScore_rec (child, myClothing);
		}
		return childScore + myScore * (multiply ? 2.0f : 1.0f);
	}

	private float GetScore(Clothing clothing) {
		if (clothing == null)
			return 0;
		switch (clothing.setID) {
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
			return 100; // Solid clothes
		case 10:
			return 150; // Stripes
		case 20:
			return 200; // Dots
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
			return 300; // Uniforms
		case 40:
		case 41:
		case 42:
			return 500; // Special
		default:
			return 100;
		}
	}
}
