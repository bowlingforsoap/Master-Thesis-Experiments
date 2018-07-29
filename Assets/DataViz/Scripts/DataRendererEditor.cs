using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;

[CustomEditor(typeof(DataRenderer))]
public class DataRendererEditor : Editor {
	private Coroutine autosaveCoroutine;
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

		if (GUILayout.Button("Take Screenshot")) {
			dataRenderer.SaveScreenshot(index);
		}

		if (GUILayout.Button("Auto Visualize and Take Screenshots")) {
			autosaveCoroutine = dataRenderer.StartCoroutine(AutoVisualizeAndTakeScreenshots(dataRenderer, dataLoader));
			
		}

		if (GUILayout.Button("Cancel Autosave")) {
			dataRenderer.StopCoroutine(autosaveCoroutine);
		}

	}

	private IEnumerator AutoVisualizeAndTakeScreenshots(DataRenderer dataRenderer, DataLoader dataLoader) {
		Debug.Log("Frame count start: " + Time.frameCount);
		Debug.Log("Entries to log: " + dataLoader.paths.Keys.ToArray().Length);
		int i = 0;
		for (i = 0; i < dataLoader.paths.Keys.ToArray().Length; i++) {
			dataRenderer.Visualize(i);
			dataRenderer.SaveScreenshot(i);
			yield return new WaitForSeconds(1f);	
		}
		Debug.Log("Entries logged: " + i);
		Debug.Log("Frame count end: " + Time.frameCount);
	}
}
