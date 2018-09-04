using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundSourceTranslationController : MonoBehaviour {
	public GameObject soundSource;
	public float soundSourceSpeed;
	public Transform center;
	public Transform leftFront, rightFront, leftBack, rightBack;//, left, right, front, back;
	public GameObject soundSourcePrefab;
	public GameObject campus;

	[SerializeField]
	private Coroutine currentCoroutine;
	[SerializeField]
	private Vector2 rectangleSize;
	[SerializeField]
	private Vector2[] rectangleCorners = new Vector2[4];
	[SerializeField]
	private List<GameObject> buildingsInCampus;


	void Start() {
		rectangleSize = new Vector2 ((leftFront.position - leftBack.position).magnitude, (leftFront.position - rightFront.position).magnitude);

		// Save rect corners as 2D points
		rectangleCorners[0] = Vector3ToVector2(leftFront.position);
		rectangleCorners[1] = Vector3ToVector2(rightFront.position);
		rectangleCorners[2] = Vector3ToVector2(rightBack.position);
		rectangleCorners[3] = Vector3ToVector2(leftBack.position);

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
	}

	public void RandomlyTranslateRandomBuilding() {
		Vector3 from;
		Vector3 to;
		
		GameObject randomBuilding = SelectRandomBuilding();
		
		AttachSoundSource(randomBuilding);
		soundSource = randomBuilding;
		
		from = soundSource.transform.position;

		Vector2 soundSourceCenter2D = Vector3ToVector2(GetGameObjectCenterInScene(soundSource));
		Debug.Log("Sound source center: " + soundSourceCenter2D);
		Debug.Log("Rectangle corner: " + rectangleCorners[0]);
		Debug.Log("Actually translating to: " + (rectangleCorners[0] - soundSourceCenter2D));

		to = Vector2ToVector3(rectangleCorners[0] - soundSourceCenter2D); // ComputeRandomTranslation(from)
		
		StartTranslateSoundSource(from, to);
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

    /* private Vector3 ComputeRandomTranslation(Vector3 from) {
		Vector3 to;

		// Generate random translation length multiplier
		float translationLengthMultiplier = UnityEngine.Random.Range(0f, 1f);
		translationLengthMultiplier = 1.5f * translationLengthMultiplier - 0.5f * translationLengthMultiplier;

		to = TranslateAlongRect(from, translationLengthMultiplier);

		return to;
	}

	private Vector3 TranslateAlongRect(Vector3 from, float translationDistance) {
		
	} */
	
	private GameObject SelectRandomBuilding() {
		return buildingsInCampus[UnityEngine.Random.Range(0, buildingsInCampus.Count)];
	}

	/* private Vector3 ProjectOntoRectangle(Vector3 point) {
		
	} */

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
