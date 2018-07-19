using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Translation {
	public Vector3 from;
	public Vector3 to;
	public string fromAbbr; // abbreviation
	public string toAbbr;
	public float timeStart;
	public float timeFinish;
}
