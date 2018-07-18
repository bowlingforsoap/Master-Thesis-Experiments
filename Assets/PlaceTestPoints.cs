using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTestPoints : MonoBehaviour {
	public Transform center;
	public Transform leftFront, rightFront, leftBack, rightBack, left, right, front, back;
	public float distanceToInscribedSquare;
	// public float distanceTocenterZ; 
	[SerializeField]
	private float radius;

	// Use this for initialization
	void Start () {
		leftFront.position = center.position + new Vector3(-distanceToInscribedSquare, 0f, distanceToInscribedSquare);
		leftFront.rotation = Quaternion.Euler(0f, -45f, 0f);
		rightFront.position = center.position + new Vector3(distanceToInscribedSquare, 0f, distanceToInscribedSquare);
		rightFront.rotation = Quaternion.Euler(0f, 45f, 0f);		
		leftBack.position = center.position + new Vector3(-distanceToInscribedSquare, 0f, -distanceToInscribedSquare);
		leftBack.rotation = Quaternion.Euler(0f, -135f, 0f);
		rightBack.position = center.position + new Vector3(distanceToInscribedSquare, 0f, -distanceToInscribedSquare);
		rightBack.rotation = Quaternion.Euler(0f, 135f, 0f);

		radius = Vector3.Distance(center.position, rightBack.position);

		front.position = center.position + new Vector3(0f, 0f, radius);
		front.localScale *= 2f;
		back.position = center.position + new Vector3(0f, 0f, -radius);
		back.rotation = Quaternion.Euler(0f, 180f, 0f);	
		left.position = center.position + new Vector3(-radius, 0f, 0f);		
		left.rotation = Quaternion.Euler(0f, -90f, 0f);
		right.position = center.position + new Vector3(radius, 0f, 0f);	
		right.rotation = Quaternion.Euler(0f, 90f, 0f);		
	}
}
