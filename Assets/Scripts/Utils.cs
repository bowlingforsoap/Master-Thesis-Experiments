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

	// Adopted from: https://stackoverflow.com/a/37406831 Author: Drew Noakes
    public struct LineSegment2f
    {
        public Vector2 From { get; set; }
        public Vector2 To { get; set; }

        public LineSegment2f(Vector2 @from, Vector2 to)
        {
            From = @from;
            To = to;
        }

        public Vector2 Delta()
        {
            return new Vector2(To.x - From.x, To.y - From.y);
        }

        /// <summary>
        /// Attempt to intersect two line segments.
        /// </summary>
        /// <remarks>
        /// Even if the line segments do not intersect, <paramref name="t"/> and <paramref name="u"/> will be set.
        /// If the lines are parallel, <paramref name="t"/> and <paramref name="u"/> are set to <see cref="float.NaN"/>.
        /// </remarks>
        /// <param name="other">The line to attempt intersection of this line with.</param>
        /// <param name="intersectionPoint">The point of intersection if within the line segments, or empty..</param>
        /// <param name="t">The distance along this line at which intersection would occur, or NaN if lines are collinear/parallel.</param>
        /// <param name="u">The distance along the other line at which intersection would occur, or NaN if lines are collinear/parallel.</param>
        /// <returns><c>true</c> if the line segments intersect, otherwise <c>false</c>.</returns>
        public bool TryIntersect(LineSegment2f other, out Vector2 intersectionPoint, out float t, out float u)
        {
            var p = From;
            var q = other.From;
            var r = Delta();
            var s = other.Delta();

            // t = (q − p) × s / (r × s)
            // u = (q − p) × r / (r × s)

            var denom = Fake2DCross(r, s);

            if (denom == 0)
            {
                // lines are collinear or parallel
                t = float.NaN;
                u = float.NaN;
                intersectionPoint = default(Vector2);
                return false;
            }

            var tNumer = Fake2DCross(q - p, s);
            var uNumer = Fake2DCross(q - p, r);

            t = tNumer / denom;
            u = uNumer / denom;

            if (t < 0 || t > 1 || u < 0 || u > 1)
            {
                // line segments do not intersect within their ranges
                intersectionPoint = default(Vector2);
                return false;
            }

            intersectionPoint = p + r * t;
            return true;
        }

        private static float Fake2DCross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
    }
}
