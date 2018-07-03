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

		PlaySound();

		while(Vector3.Distance(from, to) > Vector3.Distance(from, soundSource.transform.position)) {
			soundSource.transform.position += direction * soundSourceSpeed * Time.deltaTime;
			yield return null;
		}

		soundSource.transform.position = to;

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
