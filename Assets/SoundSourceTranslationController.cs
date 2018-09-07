using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundSourceTranslationController : MonoBehaviour {
	public float soundSourceSpeed;
	public Transform center;
	public Transform leftFront, rightFront, leftBack, rightBack;//, left, right, front, back;
	public GameObject soundSourcePrefab;
	public GameObject campus;

	[SerializeField]
	private GameObject soundSource;
	[SerializeField]
	private Coroutine currentCoroutine;
	// [SerializeField]
	// private Vector2 rectangleSize;
	// [SerializeField]
	// private Vector2[] rectangleCorners = new Vector2[4];
	[SerializeField]
	private List<GameObject> buildingsInCampus;
	private Vector3[] translationDirectionsPool;

	void Start() {
		// rectangleSize = new Vector2 ((leftFront.position - leftBack.position).magnitude, (leftFront.position - rightFront.position).magnitude);

		// Save rect corners as 2D points
		/* rectangleCorners[0] = Vector3ToVector2(leftFront.position);
		rectangleCorners[1] = Vector3ToVector2(rightFront.position);
		rectangleCorners[2] = Vector3ToVector2(rightBack.position);
		rectangleCorners[3] = Vector3ToVector2(leftBack.position); */

		// Find all buildings in campus model
		int modelsInCampus = campus.transform.childCount;
		for (int i = 0; i < modelsInCampus; i++) {
			Transform model = campus.transform.GetChild(i);
			
			if (model.name.Contains("Text") || model.name.Contains("Bridge") || model.name.Contains("Subregion") || model.name.Contains("Surface")) {
				continue;
			}

			if (model.gameObject.active) {
				buildingsInCampus.Add(model.gameObject);
			}
		}

		GenerateTranlsationDirectionsPool();
	}

	public void RandomlyTranslateRandomBuilding() {
		Vector3 from;
		Vector3 to;
		
		GameObject randomBuilding = SelectRandomBuilding();
		
		AttachSoundSource(randomBuilding);
		soundSource = randomBuilding;
		
		from = soundSource.transform.position;

		to = leftFront.position + from - GetGameObjectCenterInScene(soundSource); // rectangleCorners -> ComputeRandomTranslation(from)
		
		/* // Debug.Log("Sound source center: " + soundSourceCenter2D);
		Debug.Log("Rectangle corner: " + rectangleCorners[0]);
		Debug.Log("Actually translating to: " + to);
		
		Debugger.InstantiateAt(Vector3.zero, "------------");
		// Debugger.InstantiateAt(Vector2ToVector3(soundSourceCenter2D), "soundSource");
		Debugger.InstantiateAt(center.position, "center");
		Debugger.InstantiateAt(Vector2ToVector3(rectangleCorners[0]), "rectangleCorners[0]");
		Debugger.InstantiateAt(to, "final to");

		Debugger.ConnectIntantiatedGameObjects(); */
		
		StartTranslateSoundSource(from, to);
	}

	// Generates 100 evenly spaced direction vectors, 25 in each individual (N, E, W, S) direction
	private void GenerateTranlsationDirectionsPool() {
		translationDirectionsPool = new Vector3[100];

		Vector3 start = new Vector3(1f, 0f, 0f);
		Vector3 end = new Vector3(0f, 0f, 1f);
		float t = 1f / 26f;
		for (int i = 0; i < 25; i++) {
			translationDirectionsPool[i] = Vector3.Lerp(start, end, t * i).normalized;
		}

		Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
		for (int i = 0; i < 25; i++) {
			translationDirectionsPool[i + 25] = rotation * translationDirectionsPool[i];
		}

		rotation = Quaternion.Euler(0f, 180f, 0f);
		for (int i = 0; i < 25; i++) {
			translationDirectionsPool[i + 50] = rotation * translationDirectionsPool[i];
		}

		rotation = Quaternion.Euler(0f, 270f, 0f);
		for (int i = 0; i < 25; i++) {
			translationDirectionsPool[i + 75] = rotation * translationDirectionsPool[i];
		}

		// Debug.Log("Tranlsation Directions' Pool: " + Utils.ArrayToString<Vector3>(translationDirectionsPool));
	}

    private void AttachSoundSource(GameObject randomBuilding)
    {
        Instantiate(soundSourcePrefab, GetGameObjectCenterInScene(randomBuilding), Quaternion.identity, randomBuilding.transform);
    }

	private Vector3 GetGameObjectCenterInScene(GameObject go) {
		return go.GetComponent<Renderer>().bounds.center;
	}

	private static Vector2 Vector3ToVector2(Vector3 v3) {
		return new Vector2(v3.x, v3.z);
	}

	private static Vector3 Vector2ToVector3(Vector2 v2) {
		return new Vector3(v2.x, 0f, v2.y);
	}
	
	private GameObject SelectRandomBuilding() {
		return buildingsInCampus[UnityEngine.Random.Range(0, buildingsInCampus.Count)];
	}

	public void PositionSoundSource(Vector3 at) {
		soundSource.transform.position = at;
	}

	public void StartTranslateSoundSource(Vector3 from, Vector3 to) {
		currentCoroutine = StartCoroutine(TranslateSoundSourceCoroutine(from, to));
	}

	public void StartTranslateSoundSource(Vector3 to) {
		StartTranslateSoundSource(soundSource.transform.position, to);
	}

	public void StopTranslation() {
		if (currentCoroutine != null) {
			StopCoroutine(currentCoroutine);
			
			StopSound();
		}
	}

	private IEnumerator TranslateSoundSourceCoroutine(Vector3 from, Vector3 to) {
		Debug.Log("Started TranslateSoundSource");

		Vector3 direction = (to - from).normalized;

		if (soundSource.transform.position != from) {
			PositionSoundSource(from);
		}

		// TODO: add AudioClip to VB on the fly
		// PlaySound(); 

		// Store tranlation from
		// DataCollector.StoreTranslation(fromTransform, Time.unscaledTime);

		while(Vector3.Distance(from, to) > Vector3.Distance(from, soundSource.transform.position)) {
			soundSource.transform.position += direction * soundSourceSpeed * Time.deltaTime;
			yield return null;
		}
		soundSource.transform.position = to;

		// Store translation to
		// DataCollector.StoreTranslation(toTransform, Time.unscaledTime);

		// StopSound();

		currentCoroutine = null;
		Debug.Log("Finished TranslateSoundSource");
	}

	private void PlaySound() {
		soundSource.GetComponent<AudioSource>().Play();
	}

	private void StopSound() {
		soundSource.GetComponent<AudioSource>().Stop();
	}
}
