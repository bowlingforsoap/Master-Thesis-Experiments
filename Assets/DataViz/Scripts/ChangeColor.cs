using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

	public void ChangeColorTextAndMesh(Color color) {
		// Mesh
		GetComponent<MeshRenderer>().material.SetColor("_Color", color);
		// 3D text
		transform.GetChild(0).GetComponent<TextMesh>().color = color;
	}
}
