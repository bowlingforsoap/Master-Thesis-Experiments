using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Debugger : MonoBehaviour {
	public SoundSourceTranslationController soundSourceTranslationController;

	void Start() {
		soundSourceTranslationController.AttachSphereCollider(gameObject);
	}

	void Update() {
		if (transform.childCount == 0) {
			soundSourceTranslationController.AttachSphereCollider(gameObject);
		}
	}
}
