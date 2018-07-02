using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundSourceTranslationController))]
public class SoundSourceTranslationControllerEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		SoundSourceTranslationController controller = (SoundSourceTranslationController)target;

		GUILayout.Label("\nPosition at:");	
		if (GUILayout.Button("LF")) {
			controller.PositionSoundSource(controller.leftFront);
		}		
		if (GUILayout.Button("RF")) {
			controller.PositionSoundSource(controller.rightFront);
		}	
		if (GUILayout.Button("LB")) {
			controller.PositionSoundSource(controller.leftBack);
		}	
		if (GUILayout.Button("RB")) {
			controller.PositionSoundSource(controller.rightBack);
		}

		GUILayout.Label("\nTranslate From-To:");	
		if (GUILayout.Button("LF-RF")) {
			controller.StartTranslateSoundSource(controller.leftFront, controller.rightFront);
		}

		GUILayout.Label("\nStop:");
		if (GUILayout.Button("Stop Translation")) {
			controller.StopTranslation();
		}
	}
}
