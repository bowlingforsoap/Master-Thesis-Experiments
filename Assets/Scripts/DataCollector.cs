using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DataCollector : MonoBehaviour {
	public static Translation actualTranslation = null;
	public static Translation guess = null;

	public static bool discardLastActualTranslation = false;
	public static bool discardLastGuess = false;

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
		if (!discardLastGuess) {
			Translation prevGuess = guess;
			if (prevGuess != null) {
				userGuessesWriter.WriteLine(Utils.GuessToString(prevGuess));
			}

			discardLastGuess = false;
		}

		string selectedPole1Name = Utils.PoleName(selectedPole1);
		string selectedPole2Name = Utils.PoleName(selectedPole2);
		guess = new Translation() {from = selectedPole1.position, to = selectedPole2.position, fromAbbr = selectedPole1Name, toAbbr = selectedPole2Name};
	}

	public static void LogTranslation(Transform _from, Transform _to, float _timeStart, float _timeFinish) {
		if (!discardLastActualTranslation) {
			Translation prevTranslation = actualTranslation;
			if (prevTranslation != null) {
				actualTranslationsWriter.WriteLine(Utils.TranslationToString(prevTranslation));
			}

			discardLastActualTranslation = false;
		}

		actualTranslation = new Translation() {from = _from.position, to = _to.position, fromAbbr = _from.gameObject.name, toAbbr = _to.gameObject.name, timeStart = _timeStart, timeFinish = _timeFinish};
	}

	public static void DiscardLastActualTranslation() {
		discardLastActualTranslation = true;
	}

	public static void DiscardLastGuess() {
		discardLastGuess = true;
	}

	private static void CloseUserGuessWriter() {
		if (guess != null) {
			userGuessesWriter.WriteLine(Utils.GuessToString(guess));
		}
		if (userGuessesWriter != null) {
			userGuessesWriter.Close();
		}
	}

	private static void CloseActualTranslationWriter() {
		if (actualTranslation != null) {
			actualTranslationsWriter.WriteLine(Utils.TranslationToString(actualTranslation));
		}
		if (actualTranslationsWriter != null) {
			actualTranslationsWriter.Close();
		}
	}

	void OnApplicationQuit() {
		// Close streams
		CloseUserGuessWriter();
		if (userPositionAndOrientationWriter != null) {
			userPositionAndOrientationWriter.Close();
		}
		CloseActualTranslationWriter();
	}
}
