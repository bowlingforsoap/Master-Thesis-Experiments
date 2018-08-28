using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeManager))]
public class ShapeManagerEditor : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		ShapeManager shapeManager = (ShapeManager) target;

		if (GUILayout.Button("Next Shape")) {
			int nextShapeIndex = shapeManager.NextShapeIndex();
			Debug.Log("Next shape index: " + nextShapeIndex);
			shapeManager.ActivateShape(nextShapeIndex);
		}

		if (GUILayout.Button("Previous Shape")) {
			int prevShapeIndex = shapeManager.PrevShapeIndex();
			Debug.Log("Prev shape index: " + prevShapeIndex);
			shapeManager.ActivateShape(prevShapeIndex);
		}
	}
}
