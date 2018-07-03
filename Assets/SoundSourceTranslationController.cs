using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSourceTranslationController : MonoBehaviour {
	public GameObject soundSource;
	public float soundSourceSpeed;
	public Transform center;
	public Transform leftFront, rightFront, leftBack, rightBack;

	[SerializeField]
	private Coroutine currentCoroutine;

	public void PositionSoundSource(Transform at) {
		soundSource.transform.position = at.position;
	}

	public void StartTranslateSoundSource(Transform from, Transform to) {
		currentCoroutine = StartCoroutine(TranslateSoundSourceCoroutine(from, to));
	}

	public void StopTranslation() {
		if (currentCoroutine != null) {
			StopCoroutine(currentCoroutine);
		}
	}

	private IEnumerator TranslateSoundSourceCoroutine(Transform from, Transform to) {
		Debug.Log("Started TranslateSoundSource");		

		Vector3 direction = (to.position - from.position).normalized;

		if (soundSource.transform.position != from.position) {
			PositionSoundSource(from);
		}

		PlaySound();

		while(Vector3.Distance(from.position, to.position) > Vector3.Distance(from.position, soundSource.transform.position)) {
			soundSource.transform.position += direction * soundSourceSpeed * Time.deltaTime;
			yield return null;
		}

		soundSource.transform.position = to.position;

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
