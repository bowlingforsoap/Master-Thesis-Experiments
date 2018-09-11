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

		GUILayout.Label("Experiment Groups:");
		if (GUILayout.Button("Minimap Only")) {
			modeController.GoInMinimapOnlyMode();
		}
		if (GUILayout.Button("Sound Only")) {
			modeController.GoInSoundOnlyMode();
		}
		if (GUILayout.Button("Minimap + Sound")) {
			modeController.GoInMinimapAndSoundMode();
		}
	}
}
