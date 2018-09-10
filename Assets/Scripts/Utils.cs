using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// DataCollector Utils
public class Utils
{
    public static string GetPositionAndOrientationFromTransform(Transform transform)
    {
        string result = "";

        Vector3 position = transform.position;
        Vector3 orientation = transform.rotation.eulerAngles;
        result += Vector3ToCSV(position) + ",";
        result += Vector3ToCSV(orientation) + ",";
        result += Time.unscaledTime;
        // result += '\n';

        return result;
    }

    public static string TranslationToString(Translation translation)
    {
        string result = "";

        // 2xVector3 = 6 values + 4 = 10 values total for translation
        result += Vector3ToCSV(translation.from) + ',' + Vector3ToCSV(translation.to) + ',' + translation.fromAbbr + ',' + translation.toAbbr + ',' + translation.timeStart + ',' + translation.timeFinish;
        // result += '\n';

        return result;
    }

    public static string GuessToString(Translation guess)
    {
        string result = "";

        // 2xVector3 +2 = 8 total values
        result += Vector3ToCSV(guess.from) + ',' + Vector3ToCSV(guess.to) + ',' + guess.timeStart + ',' + guess.timeFinish;
        // result += '\n';

        return result;
    }

    public static string Vector3ToCSV(Vector3 vector3)
    {
        return vector3.x + "," + vector3.y + "," + vector3.z;
    }

    // Example
    // RB <- parent with the abbreviation name we want
    // -RB_pole <- parent
    // --default <- our transform (pole)
    public static string PoleName(Transform pole)
    {
        return pole.parent.parent.gameObject.name;
    }

    public static string ArrayToString<T>(T[] array) {
        StringBuilder sb = new StringBuilder();
        
        foreach (T t in array) {
            sb.Append(t + " ");
        }
        
        return sb.ToString();
    }

    public static List<RaycastHit> IterativelyRaycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask) {
        List<RaycastHit> raycastHits = new List<RaycastHit>();

        RaycastHit hit;
        while (Physics.Raycast(origin, direction, out hit, maxDistance)) {
            raycastHits.Add(hit);
            
            Vector3 newOrigin = hit.point;
        }

        return raycastHits;
    }

	public static void SetGameObjectsActive(GameObject[] gameObjects, bool active) {
        foreach (GameObject building in gameObjects) {
			building.gameObject.SetActive(active);
		}
	}
}
