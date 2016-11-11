using UnityEngine;

namespace Curves {
	public static class Bezier {

		//get the Vector3 point at distance t along the line
		public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
			t = Mathf.Clamp01(t);
			float oneMinusT = 1f - t;
			return
				oneMinusT * oneMinusT * p0 +
				2f * oneMinusT * t * p1 +
				t * t * p2;
		}

		//4-point version of the above function
		public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
			t = Mathf.Clamp01(t);
			float OneMinusT = 1f - t;
			return
				OneMinusT * OneMinusT * OneMinusT * p0 +
				3f * OneMinusT * OneMinusT * t * p1 +
				3f * OneMinusT * t * t * p2 +
				t * t * t * p3;
		}

		//get the tangent at distance t along the line (non-normalized)
		public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
			return
				2f * (1f - t) * (p1 - p0) +
				2f * t * (p2 - p1);
		}

		//4-point version of the above
		public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
			t = Mathf.Clamp01(t);
			float oneMinusT = 1f - t;
			return
				3f * oneMinusT * oneMinusT * (p1 - p0) +
				6f * oneMinusT * t * (p2 - p1) +
				3f * t * t * (p3 - p2);
		}

	}
}