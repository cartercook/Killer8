using UnityEngine;
using UnityEditor;
using System;

public class BezierSpline : MonoBehaviour {

	[SerializeField]
	public bool loop;

	private const int STEPS_PER_CURVE = 10;
	private const float DIRECTION_SCALE = 0.5f;

	//draw each segment of the spline in the scene view
	void OnDrawGizmos() {
		if (Selection.activeTransform != null && Selection.activeTransform.IsChildOf(transform)) {
			for (int i = 0; i < pointCount - 1; i++) {
				Vector3[] p = getCurvePoints(i);

				Handles.DrawBezier(p[0], p[3], p[1], p[2], Color.white, null, 2f);
			}

			ShowDirections();
		}
	}

	//draw little green tangent lines periodically along the curve
	void ShowDirections() {
		Handles.color = Color.green;
		Vector3 point = pointAt(0f);
		Handles.DrawLine(point, point + GetDirection(0f) * DIRECTION_SCALE);
		int steps = STEPS_PER_CURVE * (pointCount - 1);
		for (int i = 1; i <= steps; i++) {
			point = pointAt(i/(float)steps);
			Handles.DrawLine(point, point + GetDirection(i/(float)steps) * DIRECTION_SCALE);
		}
	}

	/**
	 * a spline is a series of curves strung together at the ends. Each curve is made up of 4 points:
	 * point[i], point[i]'s right extension, point[i + 1]'s left extension, and point[i + 1] 
	 */
	public int pointCount {
		get {
			//need at least 2 points to form a loop
			if (transform.childCount >= 2 && loop) {
				//act like the first point is also the last
				return transform.childCount + 1;
			} else {
				return transform.childCount;
			}
		}
	}

	//get one of the spline's control points
	public Point getPoint (int index) {
		
		if (transform.childCount >= 2) { //need at least 2 points to form a loop		 
			if (index == transform.childCount && loop) { //if index is 1 beyond childCount
				index = 0; //act like the first point is also the last
			}
		}

		return transform.GetChild(index).GetComponent<Point>(); 
	}

	//get position of a point at distance t along the line
	public Vector3 pointAt (float t) {
		//need at least two points to form a curve
		if (pointCount == 0) {
			return Vector3.zero;
		} else if (pointCount == 1) {
			return getPoint(0).transform.position;
		}

		int i;
		if (t >= 1f) { //limit t to the length of the spline
			t = 1f;
			i = pointCount - 2;
		} else {
			t = Mathf.Clamp01(t) * (pointCount - 1); //which two control points 2 lies between
			i = (int)t;
			t -= i; //get the distance along this partcular curve
		}

		Vector3[] p = getCurvePoints(i);

		return Bezier.GetPoint(p[0], p[1], p[2], p[3], t);
		//Note: this works because for the Bezier function, p[0] will always be at t=0 and p[3] will always be at t=1
	}

	public Vector3 GetVelocity (float t) {
		//need at least two points to form a curve
		if (pointCount < 2) {
			return Vector3.zero;
		}

		int i;
		if (t >= 1f) {
			t = 1f;
			i = pointCount - 2;
		} else {
			t = Mathf.Clamp01(t) * (pointCount - 1);
			i = (int)t;
			t -= i;
		}

		Vector3[] p = getCurvePoints(i);

		return Bezier.GetFirstDerivative(p[0], p[1], p[2], p[3], t);
	}

	//get the 4 points composing one segment of the spline
	public Vector3[] getCurvePoints(int index) {
		//restrict index to the second last point
		if (index > pointCount - 2)
			throw new Exception("point out of bounds");

		//get our control points
		Point point1 = getPoint(index);
		Point point4 = getPoint(index + 1);
		
		//get their right and left extension points
		Vector3 point2 = point1.transform.position + point1.transform.right * point1.rightExtension;
		Vector3 point3 = point4.transform.position - point4.transform.right * point4.leftExtension;


		//return these points
		return new Vector3[]{point1.transform.position, point2, point3, point4.transform.position};
	}
	
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

	public void AddCurve () {
		GameObject newPoint = new GameObject();
		newPoint.AddComponent<Point>();
		newPoint.transform.parent = this.transform;
		newPoint.name = "handle";
		Instantiate(newPoint);
	}
	
	public void Reset () {
		
	}
}