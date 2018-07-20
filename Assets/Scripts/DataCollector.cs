using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class DataCollector : MonoBehaviour
{
	private static DataCollector dataCollector;
	[SerializeField]
    public  static Translation actualTranslation = new Translation();
    [SerializeField]
	public static Translation guess = new Translation();
    private static Translation prevGuess;
    private static Translation prevTranslation;

    public  static bool discardLastActualTranslation = false;
    public  static bool discardLastGuess = false;

    public string actualTranslationAndGuessFilePath;
    public string userPositionAndOrientationFilePath;

    public Transform headTransform;
	public Transform center;
	public Transform front;
    [SerializeField]
    private int actualTranslationAndGuessFileEntries = 0;

    private static StreamWriter actualTranslationAndGuessWriter;
    private static StreamWriter userPositionAndOrientationWriter;

    // private StringBuilder guess = new StringBuilder();
    private StringBuilder positionAndOrientation = new StringBuilder();

    void Start()
    {
		dataCollector = this;

        // Open streams
        actualTranslationAndGuessWriter = new StreamWriter(actualTranslationAndGuessFilePath, false);
        userPositionAndOrientationWriter = new StreamWriter(userPositionAndOrientationFilePath, false);

        actualTranslationAndGuessWriter.WriteLine("From X, From Y, From Z, To X, To Y, To Z, From Abbreviation, To Abbreviation, Translation Start, Translation Finish, " 
            + "Guess From X, Guess From Y, Guess From Z, Guess To X, Guess To Y, Guess To Z, Guess Start, Guess Finish, "
            + "Angle From, Angle To, Guess Angle From, Guess Angle To, Divergence Angle From, Divergence Angle To");
        userPositionAndOrientationWriter.WriteLine("Position X, Position Y, Position Z, Rotation X, Rotation Y, Rotation Z, Time");
    }

    void Update()
    {
        LogPositionAndOrientation(headTransform);
        // userPositionAndOrientationWriter.Flush();	
    }


    private void LogPositionAndOrientation(Transform headTransform)
    {
        userPositionAndOrientationWriter.WriteLine(Utils.GetPositionAndOrientationFromTransform(headTransform));
    }

    private static void LogActualTranslationAndGuess()
    {
		//TODO: Check for null and add Discard for prev translation and guess
		/* if (discardLastGuess) {
			guess = new Translation();
			discardLastGuess = false;
			return;
		}

		if (discardLastActualTranslation) {
			actualTranslation = new Translation();
			discardLastActualTranslation = false;
			return;
		} */

		if (guess != null && actualTranslation != null && guess.from != Vector3.zero && guess.to != Vector3.zero && actualTranslation.from != Vector3.zero && actualTranslation.to != Vector3.zero)
        {
            LogPrevTranslationAndGuess();

            prevTranslation = actualTranslation;
            prevGuess = guess;

            ClearCurrentAnswer();
        }
    }

    private static void LogPrevTranslationAndGuess()
    {
        if (prevGuess != null && prevTranslation != null && prevGuess.from != Vector3.zero && prevGuess.to != Vector3.zero && prevTranslation.from != Vector3.zero && prevTranslation.to != Vector3.zero)
        {
            StringBuilder actualTranslationAndGuess = new StringBuilder();

            float fromAngle = ComputeAngleXZ(dataCollector.front.position, prevTranslation.from);
            float toAngle = ComputeAngleXZ(dataCollector.front.position, prevTranslation.to);
            float guessFromAngle = ComputeAngleXZ(dataCollector.front.position, prevGuess.from);
            float guessToAngle = ComputeAngleXZ(dataCollector.front.position, prevGuess.to);
            float fromDivergence = ComputeAngleXZ(prevTranslation.from, prevGuess.from);
            float toDivergence = ComputeAngleXZ(prevTranslation.to, prevGuess.to);

            actualTranslationAndGuess.Append(Utils.TranslationToString(prevTranslation)); // 10 values
            actualTranslationAndGuess.Append(',');
            actualTranslationAndGuess.Append(Utils.GuessToString(prevGuess)); // 8 values
            actualTranslationAndGuess.Append(',');
            actualTranslationAndGuess.Append(fromAngle);
            actualTranslationAndGuess.Append(',');
            actualTranslationAndGuess.Append(toAngle);
            actualTranslationAndGuess.Append(',');
            actualTranslationAndGuess.Append(guessFromAngle);
            actualTranslationAndGuess.Append(',');
            actualTranslationAndGuess.Append(guessToAngle);
            actualTranslationAndGuess.Append(',');
            actualTranslationAndGuess.Append(fromDivergence);
            actualTranslationAndGuess.Append(',');
            actualTranslationAndGuess.Append(toDivergence);

            actualTranslationAndGuessWriter.WriteLine(actualTranslationAndGuess.ToString()); // 24 total values per line
            
        }
        
        dataCollector.actualTranslationAndGuessFileEntries++;
    }

    public static void StoreGuess(Vector3 point, float time)
    {
        if (guess != null && guess.from == Vector3.zero)
        {
            StoreGuessFrom(point, time);
        }
        else if (guess != null && guess.from != Vector3.zero)
        {
            StoreGuessTo(point, time);
        }
    }

	public static void StoreTranslation(Transform transform, float time) {
		if (actualTranslation != null && actualTranslation.from == Vector3.zero) {
			StoreTranslationFrom(transform, time);
		} else if (actualTranslation != null && actualTranslation.from != Vector3.zero) {
			StoreTranslationTo(transform, time);
		}
	}

    private static void StoreGuessFrom(Vector3 _from, float _timeStart)
    {
        if (guess != null)
        {
            guess.from = _from;
            guess.timeStart = _timeStart;
        }
        else
        {
            guess = new Translation() { from = _from, timeStart = _timeStart };
        }
    }

    private static void StoreGuessTo(Vector3 _to, float _timeFinish)
    {
        if (guess != null)
        {
            guess.to = _to;
            guess.timeFinish = _timeFinish;

			// Try to log answer
			if (actualTranslation != null && actualTranslation.from != null && actualTranslation.to != null) {
				LogActualTranslationAndGuess();
			}
        }		
    }


    private static void StoreTranslationFrom(Transform _from, float _timeStart)
    {
        if (actualTranslation != null)
        {
            actualTranslation.from = _from.position;
            actualTranslation.fromAbbr = _from.gameObject.name;
            actualTranslation.timeStart = _timeStart;
        }
        else
        {
            actualTranslation = new Translation() { from = _from.position, fromAbbr = _from.gameObject.name, timeStart = _timeStart, };
        }
    }

    private static void StoreTranslationTo(Transform _to, float _timeFinish)
    {
        if (actualTranslation != null)
        {
            actualTranslation.to = _to.position;
            actualTranslation.toAbbr = _to.gameObject.name;
            actualTranslation.timeFinish = _timeFinish;
		
			// Try to log answer
			if (guess != null && guess.from != null && guess.to != null) { // Assume prev. translation was discarded, and log
				LogActualTranslationAndGuess();
			}
        }
    }

    private static float ComputeAngleXZ(Vector3 circlePoint1, Vector3 circlePoint2)
    {
		Vector3 from, to;

		from = circlePoint1 - dataCollector.center.position;
		to = circlePoint2 - dataCollector.center.position;

		from.y = 0f;
		to.y = 0f;

		return Vector3.SignedAngle(from, to, Vector3.up);
    }
    private static void ClearCurrentAnswer()
    {
        actualTranslation = new Translation();
        guess = new Translation();
    }
	
    public static void DiscardLastActualTranslation()
    {
        discardLastActualTranslation = true;
    }

    public static void DiscardLastGuess()
    {
        discardLastGuess = true;
    }

    private  void CloseActualTranslationUserGuessWriter()
    {
		// Log last
        if (prevGuess != null && prevTranslation != null)
        {
			LogPrevTranslationAndGuess();
        }

		// Close
        if (actualTranslationAndGuessWriter != null)
        {
            actualTranslationAndGuessWriter.Close();
        }
    }

	// Close streams
    void OnApplicationQuit()
    {
        CloseActualTranslationUserGuessWriter();
        
		if (userPositionAndOrientationWriter != null)
        {
            userPositionAndOrientationWriter.Close();
        }
    }
}
