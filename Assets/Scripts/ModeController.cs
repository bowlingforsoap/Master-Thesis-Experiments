using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour {

	public GameObject virtualBuilding;
	public GameObject guessCyclinder;
	// public GameObject campus;

	// Use this for initialization
	void Start () {
		GoInTutorialMode();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoInTutorialMode() {
		virtualBuilding.SetActive(true);
		guessCyclinder.SetActive(false);
		// campus.SetActive(true);
	}

	public void GoInExperimentMode() {
		virtualBuilding.SetActive(false);
		guessCyclinder.SetActive(true);
		// campus.SetActive(false);
	}

}
