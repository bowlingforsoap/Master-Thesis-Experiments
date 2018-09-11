using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OutlineUtils
{

    public static void ChangeOutlineColor(GameObject outlinedObject, int newOutlineColor)
    {
        try
        {
            cakeslice.Outline outline = outlinedObject.GetComponent<cakeslice.Outline>();
            outline.color = newOutlineColor;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
