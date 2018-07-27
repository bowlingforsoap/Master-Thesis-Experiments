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

		GUILayout.Label("Choose the path to visualize:");
		index = EditorGUILayout.Popup(index, dataLoader.paths.Keys.ToArray());

		if (GUILayout.Button("Visualize")) {
			dataRenderer.Visualize(index);
		}
	}
}
