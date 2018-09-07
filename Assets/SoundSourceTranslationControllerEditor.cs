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

        if (GUILayout.Button("Randomly Translate Random Building")) {
            controller.StopTranslation(); // stop previous translation, if in progress

            controller.RandomlyTranslateRandomBuilding();
        }

        // Positioning
        /* GUILayout.Label("\nPosition at:");
        if (GUILayout.Button("LF"))
        {
            controller.PositionSoundSource(controller.leftFront.position);
        }
        if (GUILayout.Button("RF"))
        {
            controller.PositionSoundSource(controller.rightFront.position);
        }
        if (GUILayout.Button("LB"))
        {
            controller.PositionSoundSource(controller.leftBack.position);
        }
        if (GUILayout.Button("RB"))
        {
            controller.PositionSoundSource(controller.rightBack.position);
        }
        if (GUILayout.Button("C"))
        {
            controller.PositionSoundSource(controller.center.position);
        }

        // Translations all
        GUILayout.Label("\nTransition to:");
        if (GUILayout.Button("LF"))
        {
            controller.StartTranslateSoundSource(controller.leftFront.position);
        }
        if (GUILayout.Button("RF"))
        {
            controller.StartTranslateSoundSource(controller.rightFront.position);
        }
        if (GUILayout.Button("LB"))
        {
            controller.StartTranslateSoundSource(controller.leftBack.position);
        }
        if (GUILayout.Button("RB"))
        {
            controller.StartTranslateSoundSource(controller.rightBack.position);
        }
        if (GUILayout.Button("C"))
        {
            controller.StartTranslateSoundSource(controller.center.position);
        } */

        // Stop
        // GUILayout.Label("\nStop:");
        if (GUILayout.Button("Stop Translation"))
        {
            controller.StopTranslation();
        } 
    }

    int RecomputeIndex(int index, int begin, int size)
    {

        return 0;
    }
}
