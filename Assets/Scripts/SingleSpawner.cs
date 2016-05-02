using UnityEngine;
using System.Collections;

public class SingleSpawner : MonoBehaviour {

	private GameObject spawn;
	public void Init(GameObject spawn) {
		this.spawn = spawn;
	}

	public void Release() {
		this.GetComponent<AnchoredJoint2D> ().connectedBody = null;
		Destroy (this.gameObject);
	}

	public void OnAnimationStart() {
		spawn.SetActive (true);
		AnchoredJoint2D conn = this.GetComponent<AnchoredJoint2D> ();
		conn.connectedBody = spawn.GetComponent<Rigidbody2D> ();
		conn.anchor = new Vector2 ();
		conn.connectedAnchor = new Vector2 ();
		spawn.SendMessage ("SetHolder", this.gameObject);

		this.spawn.transform.position = this.transform.position + new Vector3(0, - ((SpringJoint2D)conn).distance, 0);
		spawn.GetComponent<Rigidbody2D> ().isKinematic = false;
	}

	public void OnAnimationEnd() {
		Destroy (spawn);
		Destroy (this.gameObject);
	}
}
