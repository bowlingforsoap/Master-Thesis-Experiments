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
	public TextMesh stats3DText;
	public TextMesh path3DText;
	private const string statsDefaultText = "STATS (deg.)";
	private const string pathDefaultText = "PATH";

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
		// Clean stats
		CleanStatsVisualization();
		

		string pathString = dataLoader.pathNameFromIndex(pathIndex);
		DataLoader.Path path = dataLoader.paths[pathString];
		List<DataLoader.Guess> guesses = dataLoader.guessesForPath[pathString];

		// Render guesses
		foreach (DataLoader.Guess guess in guesses) {
			instantiatedObjects.Add(InstantiateLineRenderer(guessLineRendererPrefab, guess.guessedPath));
		}

		// Render actual path
		instantiatedObjects.Add(InstantiateLineRenderer(pathLineRendererPrefab, path));
		
		GameObject temp;
		temp = Instantiate(pathPointPrefab, path.from, Quaternion.identity);
		temp.GetComponent<ChangePathPointText>().ChangeText("From\n");
		instantiatedObjects.Add(temp);
		temp = Instantiate(pathPointPrefab, path.to, Quaternion.identity);
		temp.GetComponent<ChangePathPointText>().ChangeText("To\n");
		instantiatedObjects.Add(temp);

		// Render Stats
		VisualizeStats(pathIndex);
	}

	public void VisualizeStats(int pathIndex) {
		stats3DText.text += "\nmean: " + dataLoader.mean(pathIndex);
		stats3DText.text += "\nsd: " + dataLoader.sd(pathIndex);

		path3DText.text += "\nabbr.: " + dataLoader.GetPath(pathIndex).abbreviation;
		path3DText.text += "\nact.: " + dataLoader.GetPath(pathIndex).fromToAngles;
	}

	public void CleanStatsVisualization() {
		stats3DText.text = statsDefaultText;
		path3DText.text = pathDefaultText;
	}

	public void SaveScreenshot(int index) {
			ScreenCapture.CaptureScreenshot("Generated/" + dataLoader.pathNameFromIndex(index) + ".png", 4);
	}

	private GameObject InstantiateLineRenderer(GameObject prefab, DataLoader.Path line) {
		LineRenderer lineRenderer;

		lineRenderer = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, line.from);
		lineRenderer.SetPosition(1, line.to);

		return lineRenderer.gameObject;
	}
}
