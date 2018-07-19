using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ModeController))]
public class ModelControllerEditor : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		ModeController modeController = (ModeController) target;
		if (GUILayout.Button("Tutorial Mode")) {
			modeController.GoInTutorialMode();
		}
		if (GUILayout.Button("Experiment Mode")) {
			modeController.GoInExperimentMode();
		}
	}
}
