using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour {
	public GameObject emptyPrefab;
	public GameObject lineRendererPrefab;

	private static Debugger debugger;
	private List<GameObject> instantiated = new List<GameObject>();

	void Start() {
		debugger = this;

		// lineRenderer = GetComponent<LineRenderer>();
	} 

	// Instantiate an empty with a the given name at a given 3D position.
	public static void InstantiateEmptyAt(Vector3 position, string name) {
		GameObject newGameObject = Instantiate(debugger.emptyPrefab, position, Quaternion.identity);
		newGameObject.name = name;

		debugger.instantiated.Add(newGameObject);
	}

	public static void ConnectIntantiatedGameObjects(bool clearInstantiationHistory) {
		LineRenderer lineRenderer;

		lineRenderer = Instantiate(debugger.lineRendererPrefab, Vector3.zero, Quaternion.identity, debugger.transform).GetComponent<LineRenderer>();
		lineRenderer.positionCount = debugger.instantiated.Count;
		for (int i = 0; i < lineRenderer.positionCount; i++) {
			lineRenderer.SetPosition(i, debugger.instantiated[i].transform.position);
		}

		if (clearInstantiationHistory) {
			ClearInstantiationHistory();
		}
	}
	
	public static void ClearInstantiationHistory() {
		debugger.instantiated.Clear();
	}

	public static void InstantiateHierarchyDelimiter(string delimiter) {
		InstantiateEmptyAt(Vector3.zero, delimiter);
		ClearInstantiationHistory();
	}
}
