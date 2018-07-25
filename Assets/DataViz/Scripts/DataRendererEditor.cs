using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;

[CustomEditor(typeof(DataRenderer))]
public class DataRendererEditor : Editor {
	private int index;
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		DataRenderer dataRenderer = (DataRenderer) target;
		DataLoader dataLoader = dataRenderer.GetComponent<DataLoader>();

		var keys = dataLoader.paths.Keys;
		index = EditorGUILayout.Popup(index, keys.ToArray());
	}
}
