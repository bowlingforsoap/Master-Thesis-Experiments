using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class DataLoader : MonoBehaviour {
	public string[] datafilesNames;

	public struct Line {
		public Vector3 from;
		public Vector3 to;
	}

	public struct Guess {
		public int userId;
		public Line guessedPath;
	}

	private StreamReader dataReader;

	public Dictionary<string, Line> paths = new Dictionary<string, Line>(40);
	public Dictionary<string, List<Guess>> guessesForPath = new Dictionary<string, List<Guess>>();

	// Use this for initialization
	void Start () {
		int _userId = 0;
		foreach (string fileName in datafilesNames) {
			using (dataReader = new StreamReader(fileName)) {
				string data = dataReader.ReadToEnd();
				
				string[] rows = data.Split('\n');
				for (int i = 1; i < rows.Length; i++) {
					// Debug.Log(rows[i]);
					string[] values = rows[i].Split(',');
					// Debug.Log("values.Length: " + values.Length);

					try {
						// Parse paths
						string path = values[6] + "-" + values[7]; // from-to abbreviation
						Vector3 _from = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
						Vector3 _to = new Vector3(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]));
						try {
							paths.Add(path, new Line(){from = _from, to = _to});
						} catch (InvalidOperationException) {

						}

						// Parse guesses
						Vector3 _fromGuess = new Vector3(float.Parse(values[10]), float.Parse(values[11]), float.Parse(values[12]));
						Vector3 _toGuess = new Vector3(float.Parse(values[13]), float.Parse(values[14]), float.Parse(values[15]));
						Guess guess = new Guess() {userId = _userId, guessedPath = new Line() {from = _fromGuess, to = _toGuess}};
						try {
							guessesForPath.Add(path, new List<Guess>(datafilesNames.Length));
						} catch (InvalidOperationException) {}
						guessesForPath[path].Add(guess);
					} catch (IndexOutOfRangeException) {}
				}
			}

			_userId++;
		}

		GetComponent<DataRenderer>().DataIsReady = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
