using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTestPoints : MonoBehaviour {
	public Transform center;
	public Transform leftFront, rightFront, leftBack, rightBack;
	public float distanceToCenterX;
	public float distanceToCenterZ; 

	// Use this for initialization
	void Start () {
		leftFront.position = center.position + new Vector3(-distanceToCenterX, 0f, distanceToCenterZ);
		rightFront.position = center.position + new Vector3(distanceToCenterX, 0f, distanceToCenterZ);
		leftBack.position = center.position + new Vector3(-distanceToCenterX, 0f, -distanceToCenterZ);
		rightBack.position = center.position + new Vector3(distanceToCenterX, 0f, -distanceToCenterZ);
	}
}
