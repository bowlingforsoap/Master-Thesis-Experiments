using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBuildingsColor : MonoBehaviour {
	public Shader shader;
	public Color[] colors;
	
	private Material[] materials;

	void Start () {
		// Setup materials for each color
		materials = new Material[colors.Length];
		// Debug
		Dictionary<int, int> colorCounts = new Dictionary<int, int>();

		for (int i = 0; i < colors.Length; i++) {
			// Debug
			Debug.Log(colors[i]);
			colorCounts.Add(i, 0);

			Material temp = new Material(shader);
			temp.color = colors[i];
			materials[i] = temp;
		}

		// Change buildings' colors
		int numChildren = transform.childCount;
		Transform child;
		for (int i = 0; i < numChildren; i++) {
			child = transform.GetChild(i);
			string childName = child.name;
			
			if (childName.Contains("Text") || childName.Contains("Path") || childName.Contains("Subregion") || childName.Contains("Surface")) {
				continue;
			}

			int randColorIndex = Random.Range(0, materials.Length);
			child.GetComponent<MeshRenderer>().material = materials[randColorIndex];

			//Debug
			colorCounts[randColorIndex]++;
		}

		//Debug
		foreach (var colorCount in colorCounts) {
			Debug.Log(colorCount.Key + ": " + colorCount.Value);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
