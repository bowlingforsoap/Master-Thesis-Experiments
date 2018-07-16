using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DataCollector : MonoBehaviour {
	/* public struct Guess {
		public Vector3 from;
		public Vector3 to;
		public Vector3 guessFrom;
		public Vector3 guessTo;
		public string fromAbbr; // abbreviation
		public string toAbbr;
		public string guessFromAbbr;
		public string guessToAbbr;
		public long timeStart;
		public long timeFinish;
	} */

	public struct Translation {
		public Vector3 from;
		public Vector3 to;
		public string fromAbbr; // abbreviation
		public string toAbbr;
		public float timeStart;
		public float timeFinish;
	}
	public Translation actualTranslation;
	public Translation guess;

	public string userGuessesFilePath;
	public string userPositionAndOrientationFilePath;
	public string actualTranslationsFilePath;

	public Transform headTransform;

	private static StreamWriter userGuessesWriter;
	private static StreamWriter userPositionAndOrientationWriter;
	private static StreamWriter actualTranslationsWriter;

	// private StringBuilder guess = new StringBuilder();
	private StringBuilder positionAndOrientation = new StringBuilder();

	void Start() {
		// Open streams
		userGuessesWriter = new StreamWriter(userGuessesFilePath, false);
		userPositionAndOrientationWriter = new StreamWriter(userPositionAndOrientationFilePath, false);
		actualTranslationsWriter = new StreamWriter(actualTranslationsFilePath, false);

		userGuessesWriter.WriteLine("Guess From X, Guess From Y, Guess From Z, Guess To X, Guess To Y, Guess To Z, Guess From Abbreviation, Guess To Abbreviation");
		userPositionAndOrientationWriter.WriteLine("Position X, Position Y, Position Z, Rotation X, Rotation Y, Rotation Z, Time");
		actualTranslationsWriter.WriteLine("From X, From Y, From Z, To X, To Y, To Z, From Abbreviation, To Abbreviation, Time Start, Time Finish");
	}

	void Update() {
		userPositionAndOrientationWriter.WriteLine(GetPositionAndOrientationFromTransform(headTransform));
		userPositionAndOrientationWriter.Flush();	
	}

	private string GetPositionAndOrientationFromTransform(Transform transform) {
		string result = "";

		Vector3 position = transform.position;
		Vector3 orientation = transform.rotation.eulerAngles;
		result += Vector3ToCSV(position) + ",";
		result += Vector3ToCSV(orientation) + ",";
		result += Time.unscaledTime;
		// result += '\n';

		return result;
	}

	public static void LogGuess(Transform selectedPole1, Transform selectedPole2) {
		string selectedPole1Name = PoleName(selectedPole1);
		string selectedPole2Name = PoleName(selectedPole2);
		Translation guess = new Translation() {from = selectedPole1.position, to = selectedPole2.position, fromAbbr = selectedPole1Name, toAbbr = selectedPole2Name};

		userGuessesWriter.WriteLine(GuessToString(guess));
	}

	public static void LogTranslation(Transform _from, Transform _to, float _timeStart, float _timeFinish) {
		Translation translation = new Translation() {from = _from.position, to = _to.position, fromAbbr = _from.gameObject.name, toAbbr = _to.gameObject.name, timeStart = _timeStart, timeFinish = _timeFinish};
		actualTranslationsWriter.WriteLine(TranslationToString(translation));
	}

	private static string TranslationToString(Translation translation) {
		string result = "";

		result += Vector3ToCSV(translation.from)+','+Vector3ToCSV(translation.to)+','+translation.fromAbbr+','+translation.toAbbr+','+translation.timeStart+','+translation.timeFinish;
		// result += '\n';

		return result;
	}

	private static string GuessToString(Translation guess) {
		string result = "";

		result += Vector3ToCSV(guess.from)+','+Vector3ToCSV(guess.to)+','+guess.fromAbbr+','+guess.toAbbr;
		// result += '\n';

		return result;
	}

	private static string Vector3ToCSV(Vector3 vector3) {
		return vector3.x + "," + vector3.y + "," + vector3.z;
	}

	// Example
	// RB <- parent with the abbreviation name we want
	// -RB_pole <- parent
	// --default <- our transform (pole)
	private static string PoleName(Transform pole) {
		return pole.parent.parent.gameObject.name;
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
		if (actualTranslationsWriter != null) {
			actualTranslationsWriter.Close();
		}
	}
}
