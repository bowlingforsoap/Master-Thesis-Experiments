using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DataCollector Utils
public class Utils {
	public static string GetPositionAndOrientationFromTransform(Transform transform) {
		string result = "";

		Vector3 position = transform.position;
		Vector3 orientation = transform.rotation.eulerAngles;
		result += Vector3ToCSV(position) + ",";
		result += Vector3ToCSV(orientation) + ",";
		result += Time.unscaledTime;
		// result += '\n';

		return result;
	}

	public static string TranslationToString(Translation translation) {
		string result = "";

		result += Vector3ToCSV(translation.from)+','+Vector3ToCSV(translation.to)+','+translation.fromAbbr+','+translation.toAbbr+','+translation.timeStart+','+translation.timeFinish;
		// result += '\n';

		return result;
	}

	public static string GuessToString(Translation guess) {
		string result = "";

		result += Vector3ToCSV(guess.from)+','+Vector3ToCSV(guess.to)+','+guess.fromAbbr+','+guess.toAbbr;
		// result += '\n';

		return result;
	}

	public static string Vector3ToCSV(Vector3 vector3) {
		return vector3.x + "," + vector3.y + "," + vector3.z;
	}

	// Example
	// RB <- parent with the abbreviation name we want
	// -RB_pole <- parent
	// --default <- our transform (pole)
	public static string PoleName(Transform pole) {
		return pole.parent.parent.gameObject.name;
	}
}
