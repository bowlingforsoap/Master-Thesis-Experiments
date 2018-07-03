using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTestPoints : MonoBehaviour {
	public Transform center;
	public Transform leftFront, rightFront, leftBack, rightBack, left, right, front, back;
	public float distanceTocenterX;
	public float distanceTocenterZ; 

	// Use this for initialization
	void Start () {
		leftFront.position = center.position + new Vector3(-distanceTocenterX, 0f, distanceTocenterZ);
		rightFront.position = center.position + new Vector3(distanceTocenterX, 0f, distanceTocenterZ);
		leftBack.position = center.position + new Vector3(-distanceTocenterX, 0f, -distanceTocenterZ);
		rightBack.position = center.position + new Vector3(distanceTocenterX, 0f, -distanceTocenterZ);

		left.position = center.position + new Vector3(-distanceTocenterX, 0f, 0f);		
		right.position = center.position + new Vector3(distanceTocenterX, 0f, 0f);	
		front.position = center.position + new Vector3(0f, 0f, distanceTocenterZ);
		back.position = center.position + new Vector3(0f, 0f, -distanceTocenterZ);	
	}
}
