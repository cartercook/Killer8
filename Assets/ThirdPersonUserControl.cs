using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using Curves;
using UnityStandardAssets.Characters.ThirdPerson;
[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
	public BezierSpline spline;
	public float speed;
	private float t = 0;

	private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
	Rigidbody m_Rigidbody;
	Animator m_Animator;

	private void Start()
	{
		m_Character = GetComponent<ThirdPersonCharacter>();
		m_Animator = GetComponentInChildren<Animator>();
		m_Rigidbody = GetComponentInChildren<Rigidbody>();
	}

	// Fixed update is called in sync with physics
	private void FixedUpdate() {
		Vector3 splineTangent = Vector3.ProjectOnPlane(spline.GetVelocity(t), Vector3.up);

		//advance t along the line
		if (Input.GetKey(KeyCode.UpArrow)) {
			t += speed/splineTangent.magnitude;
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			t -= speed/splineTangent.magnitude;
		}

		Vector3 moveVector = spline.pointAt(t) - transform.position; //vector from character to spline point

		//draw moveVector
		Debug.DrawLine(transform.position, transform.position + moveVector, Color.red);

		//project moveVector onto ground normal
		moveVector = Vector3.ProjectOnPlane(moveVector, Vector3.up);

		//apply movement and rotation
		transform.position += moveVector;
		transform.rotation = Quaternion.LookRotation(splineTangent, Vector3.up);
		Debug.Log("moveVector: "+moveVector+", y rotation: "+transform.rotation.eulerAngles.y);

		// update the animator parameters
		m_Animator.SetFloat("Forward", moveVector.magnitude);
	}
}