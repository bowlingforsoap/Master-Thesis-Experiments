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

        if (GUILayout.Button("Begin Experiment")) {
            controller.StartCoroutine(controller.RandomBuildingTranslationLoop(/* 10 * 60f, 8, 1 * 60f */));
        }

        /* if (GUILayout.Button("Randomly Translate Random Building")) {
            controller.StopTranslation(destroyChildren: true); // stop previous translation, if in progress

            controller.RandomlyTranslateRandomBuilding();
        }

        if (GUILayout.Button("Stop Translation"))
        {
            controller.StopTranslation(destroyChildren: true);
        }  */
    }

    int RecomputeIndex(int index, int begin, int size)
    {

        return 0;
    }
}
