using UnityEditor;
using UnityEngine;
using Curves;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor {

	private const float handleSize = 0.04f;
	private const float pickSize = 0.06f;

	private BezierSpline spline;
	private Transform handleTransform;
	private Quaternion handleRotation;

	//draw options in the right-hand inspector pane
	public override void OnInspectorGUI () {
		spline = target as BezierSpline;
		EditorGUI.BeginChangeCheck();
		bool loop = EditorGUILayout.Toggle("Loop", spline.loop);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(spline, "Toggle Loop");
			EditorUtility.SetDirty(spline);
			spline.loop = loop;
		}
		
		if (GUILayout.Button("Add Curve")) {
			Undo.RecordObject(spline, "Add Curve");
			spline.AddCurve();
			EditorUtility.SetDirty(spline);
		}
	}	
}