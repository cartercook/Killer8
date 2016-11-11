using UnityEngine;

namespace Curves {
	public class SplineWalker : MonoBehaviour {

		public BezierSpline spline;

		public float speed;

		private float t;

		private void Update() {
			//https://gamedev.stackexchange.com/questions/27056/how-to-achieve-uniform-speed-of-movement-on-a-bezier-curve
			//t = t + L/||tangent(t)||
			//where L is disance/frame
			t += (speed/100)/spline.GetVelocity(t).magnitude;

			if (t > 1f) {
				if (spline.loop) {
					t -= 1f;
				} else {
					t = 1f;
				}
			}

			Vector3 position = spline.pointAt(t);
			transform.localPosition = position;

			transform.LookAt(position + spline.GetDirection(t));

		}
	}
}