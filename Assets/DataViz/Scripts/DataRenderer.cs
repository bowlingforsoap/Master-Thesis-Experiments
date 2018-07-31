using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataRenderer : MonoBehaviour {
	DataLoader dataLoader;
	public GameObject pathLineRendererPrefab;
	public GameObject guessLineRendererPrefab;
	public GameObject pathPointPrefab;
	/// <summary>Stores all the objects initializaed for the particular path</summary>
	List<GameObject> instantiatedObjects;
	public Color guessLineRendererColorStart;
	public Color guessLineRendererColorEnd;
	public TextMesh stats3DText;
	public TextMesh path3DText;
	public PlaceTestPoints radiusSource;
	private const string statsDefaultText = "STATS (deg.)";
	private const string pathDefaultText = "PATH";
	

	void Start() {
		dataLoader = GetComponent<DataLoader>();
		instantiatedObjects = new List<GameObject>(dataLoader.datafilesNames.Length + 3);

		// Individual alpha is 1/number_of_participants
		guessLineRendererColorStart.a = 1f / dataLoader.datafilesNames.Length; // combined alpha if everyone guessed correctly would be 1
		guessLineRendererColorEnd.a = 1f / dataLoader.datafilesNames.Length;
		guessLineRendererPrefab.GetComponent<LineRenderer>().startColor = guessLineRendererColorStart;
		guessLineRendererPrefab.GetComponent<LineRenderer>().endColor = guessLineRendererColorEnd;
	}

	/// <summary>Visualizes user guesses and stats, if toggle is ticked</summary>
	public void Visualize(int pathIndex, bool visualizeStats) {
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
		VisualizePath(path, "From\n", "To\n", Vector3.one, Color.green);

		if (visualizeStats) {
			// Render Stats
			VisualizeStats(pathIndex);
		}
	}

	/// <summary>Used to visualize the actual path and the mean path</summary>
	public void VisualizePath(DataLoader.Path path, string startText, string endPath, Vector3 scale, Color color) {
		// The line
		LineRenderer lineRenderer = InstantiateLineRenderer(pathLineRendererPrefab, path).GetComponent<LineRenderer>();
		lineRenderer.startColor = color;
		lineRenderer.endColor = color;

		instantiatedObjects.Add(lineRenderer.gameObject);
		
		// The end points
		GameObject temp;
		Vector3 localScale;
		temp = Instantiate(pathPointPrefab, path.from, Quaternion.identity);
		temp.GetComponent<ChangePathPointText>().ChangeText(startText);
		temp.GetComponent<ChangeColor>().ChangeColorTextAndMesh(color);
		// Scale
		localScale = temp.transform.localScale;
		localScale = new Vector3(localScale.x * scale.x, localScale.y * scale.y, scale.z * localScale.z);
		temp.transform.localScale = localScale;

		instantiatedObjects.Add(temp);


		temp = Instantiate(pathPointPrefab, path.to, Quaternion.identity);
		temp.GetComponent<ChangePathPointText>().ChangeText(endPath);
		temp.GetComponent<ChangeColor>().ChangeColorTextAndMesh(color);
		// Scale
		localScale = temp.transform.localScale;
		localScale = new Vector3(localScale.x * scale.x, localScale.y * scale.y, scale.z * localScale.z);
		temp.transform.localScale = localScale;

		instantiatedObjects.Add(temp);
	}

	public void VisualizeStats(int pathIndex) {
		Vector2 mean = dataLoader.mean(pathIndex);
		Vector2 sd = dataLoader.sd(pathIndex);

		stats3DText.text += "\nmean: " + mean;
		stats3DText.text += "\nsd: " + sd;

		path3DText.text += "\nabbr.: " + dataLoader.GetPath(pathIndex).abbreviation;
		path3DText.text += "\nact.: " + dataLoader.GetPath(pathIndex).fromToAngles;

		float radius = radiusSource.GetRadius();
		Vector3 meanFrom, meanTo;
		meanFrom = Vector3.forward * radius;
		meanTo = meanFrom;
		meanFrom = Quaternion.Euler(0f, mean.x, 0f) * meanFrom;
		meanTo = Quaternion.Euler(0f, mean.y, 0f) * meanTo;

		VisualizePath(new DataLoader.Path() {from = meanFrom, to = meanTo}, "m. From\n", "m. To\n", new Vector3(.5f, 1.5f, .5f), Color.red);
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
