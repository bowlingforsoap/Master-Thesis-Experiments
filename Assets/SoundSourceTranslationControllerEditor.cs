using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SoundSourceTranslationController))]
public class SoundSourceTranslationControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SoundSourceTranslationController controller = (SoundSourceTranslationController)target;

        // Positioning
        GUILayout.Label("\nPosition at:");
        if (GUILayout.Button("LF"))
        {
            controller.PositionSoundSource(controller.leftFront);
        }
        if (GUILayout.Button("RF"))
        {
            controller.PositionSoundSource(controller.rightFront);
        }
        if (GUILayout.Button("LB"))
        {
            controller.PositionSoundSource(controller.leftBack);
        }
        if (GUILayout.Button("RB"))
        {
            controller.PositionSoundSource(controller.rightBack);
        }
        if (GUILayout.Button("C"))
        {
            controller.PositionSoundSource(controller.center);
        }

        if (GUILayout.Button("L"))
        {
            controller.PositionSoundSource(controller.left);
        }
        if (GUILayout.Button("R"))
        {
            controller.PositionSoundSource(controller.right);
        }
        if (GUILayout.Button("F"))
        {
            controller.PositionSoundSource(controller.front);
        }
        if (GUILayout.Button("B"))
        {
            controller.PositionSoundSource(controller.back);
        }





        // Translations all
        GUILayout.Label("\nTransition to:");
        if (GUILayout.Button("LF"))
        {
            controller.StartTranslateSoundSource(controller.leftFront);
        }
        if (GUILayout.Button("RF"))
        {
            controller.StartTranslateSoundSource(controller.rightFront);
        }
        if (GUILayout.Button("LB"))
        {
            controller.StartTranslateSoundSource(controller.leftBack);
        }
        if (GUILayout.Button("RB"))
        {
            controller.StartTranslateSoundSource(controller.rightBack);
        }
        if (GUILayout.Button("C"))
        {
            controller.StartTranslateSoundSource(controller.center);
        }

        if (GUILayout.Button("L"))
        {
            controller.StartTranslateSoundSource(controller.left);
        }
        if (GUILayout.Button("R"))
        {
            controller.StartTranslateSoundSource(controller.right);
        }
        if (GUILayout.Button("F"))
        {
            controller.StartTranslateSoundSource(controller.front);
        }
        if (GUILayout.Button("B"))
        {
            controller.StartTranslateSoundSource(controller.back);
        }



        // Stop
        GUILayout.Label("\nStop:");
        if (GUILayout.Button("Stop Translation"))
        {
            controller.StopTranslation();
        }

        // Undo
        /* GUILayout.Label("\nUndo:");
		if (GUILayout.Button("Discard Last Translation")) {
			DataCollector.DiscardLastActualTranslation();
		}
		if (GUILayout.Button("Discard Last User Guess")) {
			DataCollector.DiscardLastGuess();
		} */

        // Translation Test Cases
        GUILayout.Label("\nTranslate From-To (Test cases):");

        try
        {
            foreach (KeyValuePair<string, Transform[]> button in controller.buttons)
            {

                using (new EditorGUI.DisabledScope(button.Value == null))
                {
                    if (GUILayout.Button(button.Key))
                    {
                        controller.StartTranslateSoundSource(button.Value[0], button.Value[1]);
                        controller.buttons[button.Key] = null;
                    }
                }
            }
        }
        catch (InvalidOperationException) {} // Enumerator out of sync, because we are manually changing values
    }

    int RecomputeIndex(int index, int begin, int size)
    {

        return 0;
    }
}
