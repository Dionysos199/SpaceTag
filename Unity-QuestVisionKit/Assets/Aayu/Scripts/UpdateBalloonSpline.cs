using UnityEngine;
using Dreamteck.Splines;

public class UpdateBalloonSpline : MonoBehaviour
{
    public SplineComputer spline;
    public Transform[] ropeSegments; // Transforms of the rope segment objects

    void Update()
    {
        if (spline == null || ropeSegments.Length == 0) return;

        SplinePoint[] points = spline.GetPoints();

        for (int i = 0; i < ropeSegments.Length; i++)
        {
            points[i].position = ropeSegments[i].position;
        }

        spline.SetPoints(points);
    }
}