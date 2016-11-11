using UnityEngine;
using System.Collections;

public class CameraHelper : MonoBehaviour {
	public Transform followPos; //position and orientation camera wants to assume
	public float speed;

	private CameraColliderCast colliderCast;
	new private Rigidbody rigidbody;
	new private Collider collider;

	void Start() {
		rigidbody = GetComponent<Rigidbody>();
		colliderCast = GetComponentInChildren<CameraColliderCast>();
		collider = GetComponent<Collider>();
	}

	void Update() {
		//get distance from point passed in needed to dodge any colliders
		Vector3 dodgeOffset = colliderCast.cast(followPos.position, followPos.rotation);

		//set the cameras' pos to followPos
		//offset it by dodgeOffset (which should be projected into the follow plane)
		//update dodgeOffset and smoothdamp it


		//move camera towards dodge
		Vector3 velocity = rigidbody.velocity;
		transform.position = Vector3.SmoothDamp(transform.position, followPos.position + dodgeOffset, ref velocity, 0); //smoothDamp is magic
		rigidbody.velocity = velocity;

		//update rotation
		transform.rotation = followPos.rotation;
	}
	
	/*
	//excellent debug utility for seeing collision points & normals
	void OnCollisionStay(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawLine(contact.point, contact.point+contact.normal, Color.white);
		}
	}
	*/
}