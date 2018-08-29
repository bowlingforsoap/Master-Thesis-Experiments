using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeObjExporter : MonoBehaviour {
	public string savingPathAndName;
	public ShapeManager shapeManager;

	private bool shapeSaved = false;

	public bool IsShapeSaved() {
		return shapeSaved;
	}

	public void SetShapeSaved(bool value) {
		shapeSaved = value;
	}
}
