using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour {
	public GameObject emptyPrefab;

	private static Debugger debugger;
	private static LineRenderer lineRenderer;
	private List<GameObject> instantiated = new List<GameObject>();

	void Start() {
		debugger = this;

		lineRenderer = GetComponent<LineRenderer>();
	} 

	// Instantiate an empty with a the given name at a given 3D position.
	public static void InstantiateAt(Vector3 position, string name) {
		GameObject newGameObject = Instantiate(debugger.emptyPrefab, position, Quaternion.identity);
		newGameObject.name = name;

		debugger.instantiated.Add(newGameObject);
	}

	public static void ConnectIntantiatedGameObjects() {
		lineRenderer.positionCount = debugger.instantiated.Count;

		for (int i = 0; i < lineRenderer.positionCount; i++) {
			lineRenderer.SetPosition(i, debugger.instantiated[i].transform.position);
		}

		debugger.instantiated.Clear();
	}
}
