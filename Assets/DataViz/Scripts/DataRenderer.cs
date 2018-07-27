using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataRenderer : MonoBehaviour {
	DataLoader dataLoader;
	public GameObject pathLineRendererPrefab;
	public GameObject guessLineRendererPrefab;
	public GameObject pathPointPrefab;
	List<GameObject> instantiatedObjects;
	public Color guessLineRendererColorStart;
	public Color guessLineRendererColorEnd;

	void Start() {
		dataLoader = GetComponent<DataLoader>();
		instantiatedObjects = new List<GameObject>(dataLoader.datafilesNames.Length + 3);

		guessLineRendererColorStart.a = 1f / dataLoader.datafilesNames.Length; // combined alpha if everyone guess correctly would be 1
		guessLineRendererColorEnd.a = 1f / dataLoader.datafilesNames.Length;
		guessLineRendererPrefab.GetComponent<LineRenderer>().startColor = guessLineRendererColorStart;
		guessLineRendererPrefab.GetComponent<LineRenderer>().endColor = guessLineRendererColorEnd;
	}

	public void Visualize(int pathIndex) {
		// Clean previous
		foreach (GameObject go in instantiatedObjects) {
			try {
				Destroy(go);
			} catch (MissingReferenceException) {}
		}

		string pathString = dataLoader.paths.Keys.ToArray()[pathIndex];
		DataLoader.Line path = dataLoader.paths[pathString];
		List<DataLoader.Guess> guesses = dataLoader.guessesForPath[pathString];

		// Render guesses
		foreach (DataLoader.Guess guess in guesses) {
			instantiatedObjects.Add(InstantiateLineRenderer(guessLineRendererPrefab, guess.guessedPath));
		}

		// Render actual path
		instantiatedObjects.Add(InstantiateLineRenderer(pathLineRendererPrefab, path));
		
		instantiatedObjects.Add(Instantiate(pathPointPrefab, path.from, Quaternion.identity));
		GameObject temp = Instantiate(pathPointPrefab, path.to, Quaternion.identity);
		temp.GetComponent<ChangePathPointText>().ChangeText("End\n");
		instantiatedObjects.Add(temp);
	}

	private GameObject InstantiateLineRenderer(GameObject prefab, DataLoader.Line line) {
		LineRenderer lineRenderer;

		lineRenderer = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, line.from);
		lineRenderer.SetPosition(1, line.to);

		return lineRenderer.gameObject;
	}
}
