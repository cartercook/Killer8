using UnityEngine;
using UnityEditor;
using System.Collections;

public class Point : MonoBehaviour {
	[Range(0, 10)]
	public float rightExtension = 0.75f;
	[Range(0, 10)]
	public float leftExtension = 0.75f;

	void OnDrawGizmos() {
		if (Selection.activeTransform != null && transform.parent != null && Selection.activeTransform.IsChildOf(transform.parent)) {
			//draw the middle ball in red
			Gizmos.color = new Color(1, 0.2f, 0.2f, 1);
			Gizmos.DrawSphere(transform.position, 1/8f);

			Vector3 left = transform.position - transform.right*leftExtension;
			Vector3 right = transform.position + transform.right*rightExtension;

			//draw the right ball in green
			if (leftExtension > 0) {
				Gizmos.color = new Color(0.2f, 1, 0.2f, 1);
				Gizmos.DrawSphere(left, 1/12f);
			}

			Gizmos.color = new Color(1, 1, 0.2f, 1);
			if (rightExtension > 0) {
				//draw the left ball in yellow
				Gizmos.DrawSphere(right, 1/12f);
			}

			//draw a line through all three
			Gizmos.DrawLine(left, right);
		}
	}
}
