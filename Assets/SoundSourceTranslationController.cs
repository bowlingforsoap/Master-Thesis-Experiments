using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundSourceTranslationController : MonoBehaviour {
	public GameObject soundSource;
	public float soundSourceSpeed;
	public Transform center;
	public Transform leftFront, rightFront, leftBack, rightBack, left, right, front, back;

	[SerializeField]
	private Coroutine currentCoroutine;
	public Dictionary<string, Transform[]> buttons= new Dictionary<string, Transform[]>(40);

	void Start() {
		Transform[] predefinedPositions = new Transform[8];
		predefinedPositions[0] = front;
		predefinedPositions[1] = rightFront;
		predefinedPositions[2] = right;
		predefinedPositions[3] = rightBack;
		predefinedPositions[4] = back;
		predefinedPositions[5] = leftBack;
		predefinedPositions[6] = left;
		predefinedPositions[7] = leftFront;


		// Dictionary<string, Transform[]> buttons = new Dictionary<string, Transform[]>(40);
		for (int i = 0; i < predefinedPositions.Length; i++) {
			for (int j = (i + 2) % predefinedPositions.Length; ; j = (j + 1) % predefinedPositions.Length) {
				if (j == ((i - 1) < 0 ? predefinedPositions.Length - 1 : i - 1)) {
					break;
				}
				string key;
				Transform[] value;

				// i-j
				try {
				key = predefinedPositions[i].gameObject.name + "-" + predefinedPositions[j].gameObject.name;
				value = new Transform [] {predefinedPositions[i], predefinedPositions[j]};
				buttons.Add(key, value);
				} catch (ArgumentException) {}
				// j-i
				try {
					key  = predefinedPositions[j].gameObject.name + "-" + predefinedPositions[i].gameObject.name;
					value = new Transform [] {predefinedPositions[j], predefinedPositions[i]};
					buttons.Add(key, value);
				} catch (ArgumentException) {}
			}
		}
	}

	public void PositionSoundSource(Transform at) {
		soundSource.transform.position = at.position;
	}

	public void StartTranslateSoundSource(Transform from, Transform to) {
		currentCoroutine = StartCoroutine(TranslateSoundSourceCoroutine(from, to));
	}

	public void StartTranslateSoundSource(Transform to) {
		StartTranslateSoundSource(soundSource.transform, to);
	}

	public void StopTranslation() {
		if (currentCoroutine != null) {
			StopCoroutine(currentCoroutine);
			
			StopSound();
		}
	}

	private IEnumerator TranslateSoundSourceCoroutine(Transform fromTransform, Transform toTransform) {
		Debug.Log("Started TranslateSoundSource");

		Vector3 from = fromTransform.position;
		Vector3 to = toTransform.position;	

		Vector3 direction = (to - from).normalized;

		if (soundSource.transform.position != from) {
			PositionSoundSource(fromTransform);
		}

		PlaySound();

		// Store tranlation from
		DataCollector.StoreTranslation(fromTransform, Time.unscaledTime);

		while(Vector3.Distance(from, to) > Vector3.Distance(from, soundSource.transform.position)) {
			soundSource.transform.position += direction * soundSourceSpeed * Time.deltaTime;
			yield return null;
		}
		soundSource.transform.position = to;

		// Store translation to
		DataCollector.StoreTranslation(toTransform, Time.unscaledTime);

		StopSound();

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
