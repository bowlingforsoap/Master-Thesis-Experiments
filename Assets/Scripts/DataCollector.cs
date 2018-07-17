using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DataCollector : MonoBehaviour {
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
		LogPositionAndOrientation(headTransform);
		// userPositionAndOrientationWriter.Flush();	
	}


	private static void LogPositionAndOrientation(Transform headTransform) {
		userPositionAndOrientationWriter.WriteLine(Utils.GetPositionAndOrientationFromTransform(headTransform));
	}

	public static void LogGuess(Transform selectedPole1, Transform selectedPole2) {
		string selectedPole1Name = Utils.PoleName(selectedPole1);
		string selectedPole2Name = Utils.PoleName(selectedPole2);
		Translation guess = new Translation() {from = selectedPole1.position, to = selectedPole2.position, fromAbbr = selectedPole1Name, toAbbr = selectedPole2Name};

		userGuessesWriter.WriteLine(Utils.GuessToString(guess));
	}

	public static void LogTranslation(Transform _from, Transform _to, float _timeStart, float _timeFinish) {
		Translation translation = new Translation() {from = _from.position, to = _to.position, fromAbbr = _from.gameObject.name, toAbbr = _to.gameObject.name, timeStart = _timeStart, timeFinish = _timeFinish};
		actualTranslationsWriter.WriteLine(Utils.TranslationToString(translation));
	}

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
