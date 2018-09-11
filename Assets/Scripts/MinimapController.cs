using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour {
	public GameObject minimap;
	public GameObject minimapCompass;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/* if (minimap.activeSelf && minimapCompass.activeSelf)
		{ */
			if (ModeController.Minimap) {
				minimap.SetActive(true);
				minimapCompass.SetActive(true);
			} else {
				minimap.SetActive(false);
				minimapCompass.SetActive(false);
			}
		// }
	}
}
