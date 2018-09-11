using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShapeManager : MonoBehaviour
{
    [Tooltip("Automatically set on Start from the child GameObjects. The first one is set active, others - diactivated.")]
    public GameObject[] shapes;
    public RuntimeObjExporter runtimeObjExporter;

    private int activeShapeIndex = 0;


    void Start()
    {
        shapes = new GameObject[transform.childCount];

        for (int i = 0; i < shapes.Length; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            shapes[i] = child;
            child.SetActive(false);
        }

        try
        {
            ActivateShape(activeShapeIndex);
        }
        catch (Exception e) { }
    }

    /* public int ChangeShape() {
		int nextShapeIndex = NextShapeIndex(activeShapeIndex);

		ActivateShape(nextShapeIndex);

		return nextShapeIndex;
	} */

    public void ActivateShape(int shapeIndex)
    {
        shapes[activeShapeIndex].SetActive(false);
        shapes[shapeIndex].SetActive(true);

        activeShapeIndex = shapeIndex;
    }

    public int NextShapeIndex()
    {
        return (activeShapeIndex + 1) % (shapes.Length);
    }

    public int PrevShapeIndex()
    {
        int prevIndex = activeShapeIndex - 1;

        if (prevIndex < 0)
        {
            return shapes.Length - Mathf.Abs((activeShapeIndex - 1) % (shapes.Length)); // C# modulo is a remainder apparently
        }
        else
        {
            return prevIndex;
        }
    }

    public GameObject GetActiveShape()
    {
        return shapes[activeShapeIndex];
    }
}
