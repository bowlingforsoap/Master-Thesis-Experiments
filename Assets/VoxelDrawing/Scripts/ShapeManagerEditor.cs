using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeManager))]
public class ShapeManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShapeManager shapeManager = (ShapeManager)target;

        bool switchedToNextShape = true;
        if (GUILayout.Button("Next Shape"))
        {
            if (shapeManager.runtimeObjExporter.IsShapeSaved())
            {
				VoxelDrawing.VoxelDrawer.GetVoxelDrawer().EraseAllVoxels();				

                int nextShapeIndex = shapeManager.NextShapeIndex();
                Debug.Log("Next shape index: " + nextShapeIndex);
                shapeManager.ActivateShape(nextShapeIndex);

				shapeManager.runtimeObjExporter.SetShapeSaved(false);
            }
            else
            {
				shapeManager.runtimeObjExporter.SetShapeSaved(true);
                switchedToNextShape = false;
            }
        }

        if (GUILayout.Button("Previous Shape"))
        {
            if (shapeManager.runtimeObjExporter.IsShapeSaved())
            {
				VoxelDrawing.VoxelDrawer.GetVoxelDrawer().EraseAllVoxels();

                int prevShapeIndex = shapeManager.PrevShapeIndex();
                Debug.Log("Prev shape index: " + prevShapeIndex);
                shapeManager.ActivateShape(prevShapeIndex);

				shapeManager.runtimeObjExporter.SetShapeSaved(false);
            }
            else
            {
				shapeManager.runtimeObjExporter.SetShapeSaved(true);
                switchedToNextShape = false;
            }
        }

        if (!switchedToNextShape)
        {
            Debug.Log("Mesh was not saved! Press again to switch w/o saving..");
        }
    }
}
