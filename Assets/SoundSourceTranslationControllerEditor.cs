using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundSourceTranslationController))]
public class SoundSourceTranslationControllerEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		SoundSourceTranslationController controller = (SoundSourceTranslationController)target;

		// Positioning
		GUILayout.Label("\nPosition at:");	
		if (GUILayout.Button("LF")) {
			controller.PositionSoundSource(controller.leftFront.position);
		}		
		if (GUILayout.Button("RF")) {
			controller.PositionSoundSource(controller.rightFront.position);
		}	
		if (GUILayout.Button("LB")) {
			controller.PositionSoundSource(controller.leftBack.position);
		}	
		if (GUILayout.Button("RB")) {
			controller.PositionSoundSource(controller.rightBack.position);
		}
		if (GUILayout.Button("C")) {
			controller.PositionSoundSource(controller.center.position);
		}

		if (GUILayout.Button("L")) {
			controller.PositionSoundSource(controller.left.position);
		}		
		if (GUILayout.Button("R")) {
			controller.PositionSoundSource(controller.right.position);
		}	
		if (GUILayout.Button("F")) {
			controller.PositionSoundSource(controller.front.position);
		}	
		if (GUILayout.Button("B")) {
			controller.PositionSoundSource(controller.back.position);
		}





		// Translations all
		GUILayout.Label("\nTransition to:");	
		if (GUILayout.Button("LF")) {
			controller.StartTranslateSoundSource(controller.leftFront.position);
		}		
		if (GUILayout.Button("RF")) {
			controller.StartTranslateSoundSource(controller.rightFront.position);
		}	
		if (GUILayout.Button("LB")) {
			controller.StartTranslateSoundSource(controller.leftBack.position);
		}	
		if (GUILayout.Button("RB")) {
			controller.StartTranslateSoundSource(controller.rightBack.position);
		}
		if (GUILayout.Button("C")) {
			controller.StartTranslateSoundSource(controller.center.position);
		}

		if (GUILayout.Button("L")) {
			controller.StartTranslateSoundSource(controller.left.position);
		}		
		if (GUILayout.Button("R")) {
			controller.StartTranslateSoundSource(controller.right.position);
		}	
		if (GUILayout.Button("F")) {
			controller.StartTranslateSoundSource(controller.front.position);
		}	
		if (GUILayout.Button("B")) {
			controller.StartTranslateSoundSource(controller.back.position);
		}



		// Stop
		GUILayout.Label("\nStop:");
		if (GUILayout.Button("Stop Translation")) {
			controller.StopTranslation();
		}




		// Translation Test Cases
		GUILayout.Label("\nTranslate From-To (Test cases):");	
		if (GUILayout.Button("C-RF")) {
			controller.StartTranslateSoundSource(controller.center.position, controller.rightFront.position);
		}
		if (GUILayout.Button("RF-C")) {
			controller.StartTranslateSoundSource(controller.rightFront.position, controller.center.position);
		}
		if (GUILayout.Button("C-LF")) {
			controller.StartTranslateSoundSource(controller.center.position, controller.leftFront.position);
		}
		if (GUILayout.Button("LF-C")) {
			controller.StartTranslateSoundSource(controller.leftFront.position, controller.center.position);
		}

		if (GUILayout.Button("C-RB")) {
			controller.StartTranslateSoundSource(controller.center.position, controller.rightBack.position);
		}
		if (GUILayout.Button("RB-C")) {
			controller.StartTranslateSoundSource(controller.rightBack.position, controller.center.position);
		}
		if (GUILayout.Button("C-LB")) {
			controller.StartTranslateSoundSource(controller.center.position, controller.leftBack.position);
		}
		if (GUILayout.Button("LB-C")) {
			controller.StartTranslateSoundSource(controller.leftBack.position, controller.center.position);
		}

		if (GUILayout.Button("RF-LF")) {
			controller.StartTranslateSoundSource(controller.rightFront.position, controller.leftFront.position);
		}
		if (GUILayout.Button("LF-RF")) {
			controller.StartTranslateSoundSource(controller.leftFront.position, controller.rightFront.position);
		}

		if (GUILayout.Button("LF-LB")) {
			controller.StartTranslateSoundSource(controller.leftFront.position, controller.leftBack.position);
		}
		if (GUILayout.Button("LB-LF")) {
			controller.StartTranslateSoundSource(controller.leftBack.position, controller.leftFront.position);
		}

		if (GUILayout.Button("LB-RB")) {
			controller.StartTranslateSoundSource(controller.leftBack.position, controller.rightBack.position);
		}
		if (GUILayout.Button("RB-LB")) {
			controller.StartTranslateSoundSource(controller.rightBack.position, controller.leftBack.position);
		}

		if (GUILayout.Button("RB-RF")) {
			controller.StartTranslateSoundSource(controller.rightBack.position, controller.rightFront.position);
		}
		if (GUILayout.Button("RF-RB")) {
			controller.StartTranslateSoundSource(controller.rightFront.position, controller.rightBack.position);
		}

		
	}
}
