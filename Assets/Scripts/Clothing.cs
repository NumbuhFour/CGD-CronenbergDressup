using UnityEngine;
using System.Collections.Generic;

public class Clothing : MonoBehaviour {

	private struct Connection
	{
		public GameObject go;
		public SpringJoint2D j;
	}

	[SerializeField]
	public BodyPartTypes type;
	public int setID = -1;

	private bool connected = false;

	[SerializeField]
	private TargetJoint2D mouseJoint;

	private World world;
	private GameObject death;

	[SerializeField]
	private float grabDistance = 0.3f;

	private GameObject spawner; 
	public void SetHolder(GameObject spawner) {
		this.spawner = spawner;
	}

	void Awake()
	{
		world = GameObject.Find ("World").GetComponent<World> ();
		death = GameObject.Find ("Death");

		mouseJoint.enabled = false;
	}

	void Update() {
		if (mouseJoint.enabled && !connected) {
			CheckForConnection ();
		} else if (!connected && this.transform.position.y < death.transform.position.y) {
			Destroy (this.gameObject);
		}
	}

	void OnMouseDown()
	{
		if (!connected) {
			if (this.spawner) {
				this.spawner.SendMessage ("Release");
				this.spawner = null;
			}
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

	}

	void CheckForConnection() {
		bool found = false;
		List<GameObject> conns = world.Clothes;
		foreach (GameObject go in conns) {
			Transform t = go.transform;
			Connectable conn = t.parent.parent.GetComponent<Connectable> ();
			if (conn.type == this.type && (this.transform.position - t.position).magnitude < grabDistance) {
				world.CleanConnections ();
				connected = true;
				mouseJoint.enabled = false;
				conn.AddClothes (this);
				break;
			}
		}
	}
}
