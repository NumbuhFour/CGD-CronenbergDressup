using UnityEngine;
using System.Collections.Generic;

public class Connectable : MonoBehaviour {
	[SerializeField]
	private bool startNode = false;


	private struct Connection
	{
		public GameObject go;
		public SpringJoint2D j;
	}

	[SerializeField]
	public BodyPartTypes type;
	private List<Connectable> children = new List<Connectable> ();

	[SerializeField]
	private DistanceJoint2D maxDistance;

	private bool connected = false;

	private DistanceJoint2D clothesJoint;
	private GameObject clothesPoint;
	private GameObject clothing = null;
	private List<Connection> joints = new List<Connection>();

	[SerializeField]
	private TargetJoint2D mouseJoint;

	private World world;
	private GameObject death;

	[SerializeField]
	private string dragLayer;
	private int dragLayerID;
	private int startLayerID;

	[SerializeField]
	private float grabDistance = 0.3f;

	// Use this for initialization
	void Start () {

		dragLayerID = LayerMask.NameToLayer (dragLayer);
		startLayerID = this.gameObject.layer;

		Transform connContainer = this.transform.FindChild ("_connections");
		for (int i = 0; i < connContainer.childCount; i++) {
			Transform child = connContainer.GetChild (i);
			if (child.name == "_cnc") {
				clothesPoint = child.gameObject;
				clothesJoint = this.gameObject.AddComponent<DistanceJoint2D> ();
				clothesJoint.enabled = false;
				clothesJoint.connectedAnchor = child.position;
			} else {
				SpringJoint2D joint = this.gameObject.AddComponent<SpringJoint2D> ();
				joint.enabled = false;
				joint.distance = 0.05f;
				joint.anchor = child.localPosition;
				Connection c = new Connection();
				c.go = child.gameObject;
				c.j = joint;
				joints.Add (c);
			}
		}
		if (startNode)
			return;
	}
		
	private void SetLayerRecursively(Transform t, int layer) {
		t.gameObject.layer = layer;
		for (int i = 0; i < t.childCount; i++)
			SetLayerRecursively (t.GetChild (i), layer);
	}


	void Awake()
	{
		if (startNode)
			return;
		world = GameObject.Find ("World").GetComponent<World> ();
		death = GameObject.Find ("Death");

		mouseJoint.enabled = false;
		// mouseJoint.connectedAnchor = gameObject.transform.position;

		maxDistance.enabled = false;
	}

	void Update() {
		if (startNode)
			return;

		if (this.clothing) {
			this.clothing.transform.position = this.clothesPoint.transform.position;
			this.clothing.transform.rotation = this.clothesPoint.transform.rotation;
		}

		if (mouseJoint.enabled && !connected) {
			CheckForConnection ();
		} else if (!connected && this.transform.position.y < death.transform.position.y) {
			Destroy (this.gameObject);
		}
	}

	void OnMouseDown()
	{
		if (!connected && !startNode) {
			SetLayerRecursively (this.transform, dragLayerID);
			mouseJoint.enabled = true;
			Vector2 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);//getting cursor position
			mouseJoint.target = cursorPosition;
			mouseJoint.anchor = this.transform.InverseTransformPoint (cursorPosition);
		}
	}


	void OnMouseDrag()        
	{
		if (mouseJoint.enabled) 
		{
			Vector2 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);//getting cursor position

			mouseJoint.target = cursorPosition;//the anchor get's cursor's position


		}
	}


	void OnMouseUp()        
	{

		mouseJoint.enabled = false;
		if (!startNode && !connected) {
			SetLayerRecursively (this.transform, startLayerID);
		}
	}

	void CheckForConnection() {
		bool found = false;
		List<GameObject> conns = world.Connections;
		foreach (GameObject go in conns) {
			Transform t = go.transform;
			foreach (Connection joint in this.joints) {
				if (joint.go.activeSelf && (joint.go.transform.position - t.position).magnitude < grabDistance) {
					joint.go.name = "_cnby";
					joint.j.enabled = true;
					mouseJoint.enabled = false;

					go.name = "_cnbx";
					joint.j.connectedAnchor = go.transform.position;
					joint.j.connectedBody = go.GetComponent<Rigidbody2D> ();
					joint.j.distance = 0.0f;
					world.addConnection (this.gameObject);
					this.connected = true;

					maxDistance.anchor = joint.j.anchor;
					maxDistance.connectedBody = joint.j.connectedBody;
					maxDistance.connectedAnchor = joint.j.connectedAnchor;
					maxDistance.enabled = true;

					t.parent.parent.GetComponent<Connectable> ().children.Add (this);

					found = true;
					break;
				}
			}
			if (found) break;
		}
	}

	public void AddClothes(Clothing cloth) {
		GameObject go = cloth.gameObject;
		go.transform.position = this.clothesPoint.transform.position;
		go.transform.rotation = this.clothesPoint.transform.rotation;

		// Have clothes, remove it
		if (this.clothing) {
			Destroy (this.clothing);
		}

		this.clothing = go;
		Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0;
	}
}
