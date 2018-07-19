using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSourceTranslationController : MonoBehaviour {
	public GameObject soundSource;
	public float soundSourceSpeed;
	public Transform center;
	public Transform leftFront, rightFront, leftBack, rightBack, left, right, front, back;

	[SerializeField]
	private Coroutine currentCoroutine;

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
