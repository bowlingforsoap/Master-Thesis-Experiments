using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;


public class DataLoader : MonoBehaviour {
	public string[] datafilesNames;

	public struct Path {
		public Vector3 from;
		public Vector3 to;
		public Vector2 fromToAngles; // .x - from, .y - to
		public string abbreviation;
	}

	public struct Guess {
		public int userId;
		public Path guessedPath;
	}

	private StreamReader dataReader;

	// <pathName, pathCoords>
	public Dictionary<string, Path> paths = new Dictionary<string, Path>(40);
	// <pathName, listOfGuesses>
	public Dictionary<string, List<Guess>> guessesForPath = new Dictionary<string, List<Guess>>();

	// Use this for initialization
	void Start () {
		LoadData();
	}

	private void LoadData() {
		int _userId = 0;

		foreach (string fileName in datafilesNames) {

			using (dataReader = new StreamReader(fileName)) {
				string data = dataReader.ReadToEnd();
				string[] rows = data.Split('\n');
				for (int i = 1; i < rows.Length; i++) {
					string[] values = rows[i].Split(',');

					try {
						// Parse paths
						string path = values[6] + "-" + values[7]; // from-to abbreviation
						Vector3 _from = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
						Vector3 _to = new Vector3(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]));
						Vector2 _fromToAngles = new Vector2(float.Parse(values[values.Length - 6]), float.Parse(values[values.Length - 5]));
						string _abbreviation = values[6] + "-" + values[7];

						try {
							paths.Add(path, new Path(){from = _from, to = _to, fromToAngles = _fromToAngles, abbreviation = _abbreviation});
						} catch (InvalidOperationException) {}
						catch (ArgumentException) {}

						// Parse guesses
						Vector3 _fromGuess = new Vector3(float.Parse(values[10]), float.Parse(values[11]), float.Parse(values[12]));
						Vector3 _toGuess = new Vector3(float.Parse(values[13]), float.Parse(values[14]), float.Parse(values[15]));
						float _guessedAngleFrom = float.Parse(values[values.Length - 4]);
						float _guessedAngleTo = float.Parse(values[values.Length - 3]);
						Guess guess = new Guess() {userId = _userId, guessedPath = new Path() {from = _fromGuess, to = _toGuess, fromToAngles = new Vector2(_guessedAngleFrom,_guessedAngleTo)}};

						try {
							guessesForPath.Add(path, new List<Guess>(datafilesNames.Length));
						} catch (InvalidOperationException) {}
						catch (ArgumentException) {}
						guessesForPath[path].Add(guess);
					} catch (IndexOutOfRangeException) {}
				}
			}

			_userId++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string pathNameFromIndex(int pathIndex) {
		return paths.Keys.ToArray()[pathIndex];
	}

	public Vector2 mean(int pathIndex) {
		return mean(paths.Keys.ToArray()[pathIndex]);
	}

	public Vector2 mean(string pathName) {
		List<Guess> guesses = guessesForPath[pathName];
		Vector2[] values = new Vector2[guesses.Count];

		for (int i = 0; i < values.Length; i++) {
			values[i] = guesses[i].guessedPath.fromToAngles;
		}
		
		return mean(values);
	}

	/// <summary>Math for mean angle computation: https://rosettacode.org/wiki/Averages/Mean_angle</summary>
	private Vector2 mean(Vector2[] values) {
		Vector2 result, resultX, resultY;
		result = Vector2.zero;
		resultX = Vector2.zero;
		resultY = Vector3.zero;

		foreach (Vector2 value in values) {
			// Convert to [0;360] values on the circle
			float xAngle = value.x < 0 ? (360f + value.x) : value.x;
			float yAngle = value.x < 0 ? (360f + value.y) : value.y;
			// To complex numbers	
			resultX += new Vector2(Mathf.Sin(xAngle * Mathf.PI / 180f), Mathf.Cos(xAngle * Mathf.PI / 180f));
			resultY += new Vector2(Mathf.Sin(yAngle * Mathf.PI / 180f), Mathf.Cos(yAngle * Mathf.PI / 180f));
		}
		// Mean
		result /= values.Length;
		// Back to [0;360]
		result = new Vector2(Mathf.Atan2(resultX.x, resultX.y) * 180f / Mathf.PI, Mathf.Atan2(resultY.x, resultY.y) * 180f / Mathf.PI);
		// Convert back to [-180;180]
		result.x = result.x > 180f ? result.x - 360f : result.x;
		result.y = result.y > 180f ? result.y - 360f : result.y;  

		return result;
	}

	public Vector2 sd(int pathIndex) {
		return sd(paths.Keys.ToArray()[pathIndex]);
	}

	public Vector2 sd(string pathName) {
		List<Guess> guesses = guessesForPath[pathName];
		Vector2[] values = new Vector2[guesses.Count];

		for (int i = 0; i < values.Length; i++) {
			values[i] = guesses[i].guessedPath.fromToAngles;
		}

		return sd(values);
	}

	private Vector2 sd(Vector2[] values) {
		Vector2 meanValues = mean(values);
		Vector2 squareDifferences = Vector2.zero;

		foreach (Vector2 value in values) {
			squareDifferences += new Vector2(Mathf.Pow(meanValues.x - value.x, 2f), Mathf.Pow(meanValues.y - value.y, 2f));
		}

		Vector2 result = new Vector2(Mathf.Sqrt(squareDifferences.x/values.Length), Mathf.Sqrt(squareDifferences.y/values.Length));

		return result;
	}

	public Path GetPath(int pathIndex) {
		return GetPath(pathNameFromIndex(pathIndex));
	}

	public Path GetPath(string pathName) {
		return paths[pathName];
	}
}
