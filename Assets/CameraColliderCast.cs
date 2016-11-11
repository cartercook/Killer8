using UnityEngine;
using System.Collections;

public class CameraColliderCast : MonoBehaviour {
	new private Collider collider;
	private Vector3 dodgeOffset = Vector3.zero;

	void Start () {
		collider = GetComponent<Collider>();
	}

	//returns a vector that is the distance from the cast-point the camera must go to avoid collision
	public Vector3 cast(Vector3 position, Quaternion rotation) {
		transform.position = position;
		transform.rotation = rotation;
		
		//Warning: this dodgeOffset will always be 1 frame behind
		return dodgeOffset;
	}

	void OnTriggerEnter(Collider other) { OnTrigger(other); }
	void OnTriggerStay(Collider other) { OnTrigger(other); }
	void OnTrigger(Collider other) {
		//get collider centers
		Vector3 otherCenter = other.bounds.center;
		Vector3 ourCenter = collider.bounds.center;

		//vector from other collider to closest point
		Vector3 separation = other.ClosestPointOnBounds(ourCenter) - otherCenter;

		//extend the vector by collider radius + const
		separation += separation.normalized*(collider.bounds.extents.sqrMagnitude+0.2f);

		Debug.DrawLine(otherCenter, otherCenter+separation, Color.red);

		//get the dodgePoint in world space
		Vector3 dodgePoint = otherCenter + separation;

		//set dodgeOffset
		dodgeOffset = dodgePoint - transform.position;
	}

	void OnTriggerExit(Collider other) {
		dodgeOffset = Vector3.zero;
	}
}