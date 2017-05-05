using System;
using UnityEngine;
using Curves;

public class ThirdPersonUserControl : MonoBehaviour
{
	public BezierSpline spline;
	public float speed;
	private float t = 0;

	Rigidbody rigidBody;
	Animator animator;

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
		rigidBody = GetComponentInChildren<Rigidbody>();
	}

	// Fixed update is called in sync with physics
	private void FixedUpdate() {
		bool up = Input.GetKey(KeyCode.UpArrow);
		bool down = Input.GetKey(KeyCode.DownArrow);

		Vector3 moveVector = Vector3.zero;

		if (up || down) {
			

			Vector3 splineTangent = Vector3.ProjectOnPlane(spline.GetVelocity(t), Vector3.up);

			//advance t along the line
			float deltaT = 0;
			if (up) {
				deltaT = speed/splineTangent.magnitude;
			} else if (down) {
				deltaT = -speed/splineTangent.magnitude;
			}

			t += deltaT;

			moveVector = spline.pointAt(t) - transform.position; //vector from character to spline point

			//draw moveVector
			Debug.DrawLine(transform.position, transform.position + moveVector, Color.red);

			//project moveVector onto ground normal
			moveVector = Vector3.ProjectOnPlane(moveVector, Vector3.up);

			//apply movement and rotation
			transform.position += moveVector;
			transform.rotation = Quaternion.LookRotation(Math.Sign(deltaT)*splineTangent, Vector3.up);
			Debug.Log("moveVector: "+moveVector+", y rotation: "+transform.rotation.eulerAngles.y);
		}

		// update the animator parameters
		animator.SetFloat("Forward", moveVector.magnitude);
	}
}