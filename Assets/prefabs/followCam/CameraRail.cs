using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraRail : MonoBehaviour {
	[Flags]
	public enum LockFlags {
		None,
		Horizontal,
		Vertical,
		Both
	}

	[Range(0, 25)]
	public float Force = 1;

	public LockFlags Lock = LockFlags.Both;

	public Transform Target;

	private Rigidbody _rigidbody;

	private Transform _transform;

	public void Awake() {
		_rigidbody = GetComponent<Rigidbody>();
		_transform = GetComponent<Transform>();
	}

	public void Update() {
		if (Lock == LockFlags.None || Force <= 0) return;

		bool lockX = (Lock & LockFlags.Horizontal) == LockFlags.Horizontal;
		bool lockY = (Lock & LockFlags.Vertical) == LockFlags.Vertical;

		Vector3 velocity = _transform.InverseTransformDirection(_rigidbody.velocity);
		if (lockX) velocity.x = 0;
		if (lockY) velocity.y = 0;
		_rigidbody.velocity = _transform.TransformDirection(velocity);

		Vector3 distance = Target.position - _transform.position;
		Vector3 idealPosition = Target.position - _transform.forward * distance.magnitude;
		Vector3 correction = idealPosition - _transform.position;

		correction = _transform.InverseTransformDirection(correction);
		if (!lockX) correction.x = 0;
		if (!lockY) correction.y = 0;
		correction.z = 0;
		correction = _transform.TransformDirection(correction);

		_rigidbody.velocity += correction * Force;
	}
}