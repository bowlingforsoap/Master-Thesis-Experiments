using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DataCollector : MonoBehaviour {
	public struct Guess {
		public string from;
		public string to;
		public string guessFrom;
		public string guessTo;
		public long timeStart;
		public long timeFinish;
	};

	public string userGuessesFilePath;
	public string userPositionAndOrientationFilePath;

	public Transform headTransform;

	private StreamWriter userGuessesWriter;
	private StreamWriter userPositionAndOrientationWriter;

	private StringBuilder guess = new StringBuilder();
	private StringBuilder positionAndOrientation = new StringBuilder();

	void Start() {
		// Open streams
		userGuessesWriter = new StreamWriter(userGuessesFilePath, true);
		userPositionAndOrientationWriter = new StreamWriter(userPositionAndOrientationFilePath, true);
	}

	void Update() {
		userPositionAndOrientationWriter.WriteLine(GetPositionAndOrientationFromTransform(headTransform));
	}

	private string GetPositionAndOrientationFromTransform(Transform transform) {
		string result = "";

		Vector3 position = transform.position;
		Vector3 orientation = transform.rotation.eulerAngles;
		result += position.x + "," + position.y + "," + position.z;
		result += orientation.x + "," + orientation.y + "," + orientation.z;
		result += '\n';

		return result;
	}

	public void WriteGuess(Guess guess) {
		string guessString = GuessToString(guess);

		userGuessesWriter.WriteLine(guessString);
	}

	private string GuessToString(Guess guess) {
		string result = "";

		result += guess.from+','+guess.to+','+guess.guessFrom+','+guess.guessTo+','+guess.timeStart+','+guess.timeFinish;
		result += '\n';

		return result;
	}

	/* public void AddCommaSeparatedValueToGuess(string value) {
		AddCommaSeparatedValue(ref guess, value);
	}

	private void AddCommaSeparatedValue(ref StringBuilder stringBuilder, string value) {
		stringBuilder.Append(value);
	}

	void WriteGuess() {
		userGuessesWriter.WriteLine(guess.ToString());
		guess.Clear();
	}
	void WritePositionAndOrientation(string positionAndOrientation) {
		userPositionAndOrientationWriter.WriteLine(positionAndOrientation);
	} */

	void OnApplicationQuit() {
		// Close streams
		if (userGuessesWriter != null) {
			userGuessesWriter.Close();
		}
		if (userPositionAndOrientationWriter != null) {
			userPositionAndOrientationWriter.Close();
		}
	}
}
