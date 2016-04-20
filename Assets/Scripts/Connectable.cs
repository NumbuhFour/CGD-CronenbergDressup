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
	private BodyPartTypes type;
	private List<Connectable> children = new List<Connectable> ();

	[SerializeField]
	private DistanceJoint2D maxDistance;

	private bool connected = false;

	private DistanceJoint2D clothesJoint;
	private List<Connection> joints = new List<Connection>();

	private SpringJoint2D spring;

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
		if (startNode)
			return;

		dragLayerID = LayerMask.NameToLayer (dragLayer);
		startLayerID = this.gameObject.layer;

		Transform connContainer = this.transform.FindChild ("_connections");
		for (int i = 0; i < connContainer.childCount; i++) {
			Transform child = connContainer.GetChild (i);
			if (child.name == "_cnc") {
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

		spring = this.gameObject.GetComponent<SpringJoint2D>(); //"spring" is the SpringJoint2D component that I added to my object
		spring.enabled = false;
		spring.connectedAnchor = gameObject.transform.position;//i want the anchor position to start at the object's position

		maxDistance.enabled = false;
		maxDistance.connectedAnchor = gameObject.transform.position;//i want the anchor position to start at the object's position
	}

	void Update() {
		if (startNode)
			return;
		if (maxDistance.enabled && !connected) {
			CheckForConnection ();
		} else if (!connected && this.transform.position.y < death.transform.position.y) {
			Destroy (this.gameObject);
		}
	}

	void OnMouseDown()
	{
		if (!connected && !startNode) {
			SetLayerRecursively (this.transform, dragLayerID);
			maxDistance.enabled = true;//I'm reactivating the SpringJoint2D component each time I'm clicking on my object becouse I'm disabling it after I'm releasing the mouse click so it will fly in the direction i was moving my mouse
			Vector2 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);//getting cursor position
			maxDistance.anchor = transform.InverseTransformPoint (cursorPosition);
		}
	}


	void OnMouseDrag()        
	{
		if (maxDistance.enabled) 
		{
			Vector2 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);//getting cursor position

			maxDistance.connectedAnchor = cursorPosition;//the anchor get's cursor's position


		}
	}


	void OnMouseUp()        
	{
		if (!connected && !startNode) {
			SetLayerRecursively (this.transform, startLayerID);
			maxDistance.enabled = false;//disabling the spring component
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
					spring.enabled = false;
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
}
