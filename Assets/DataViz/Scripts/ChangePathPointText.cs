using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePathPointText : MonoBehaviour {

	public void ChangeText(string newText) {
		transform.GetChild(0).GetComponent<TextMesh>().text = newText;
	}
}
