using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFieldGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int numberOfPointsToGenerate = 100;
    [SerializeField] Bounds fieldBounds;

    [Range(0.1f, 0.9f)]
    [SerializeField] float boundsInterval = 0.2f;

    public List<Point> GetPoints()
    {
        List<Point> points = Helpers.GetPointsOnBounds(fieldBounds, intervalRateNormalized: boundsInterval);

        //Creating point field
        for (int i = 0; i < numberOfPointsToGenerate; i++)
        {
            float xDistance = fieldBounds.max.x - fieldBounds.min.x;
            float yDistance = fieldBounds.max.y - fieldBounds.min.y;

            //TODO :: Add slight offset here.
            float x = Random.Range(fieldBounds.min.x + (xDistance * boundsInterval), fieldBounds.max.x - (xDistance * boundsInterval));
            float y = Random.Range(fieldBounds.min.y + (yDistance * boundsInterval), fieldBounds.max.y - (yDistance * boundsInterval));

            Point point = new Point(x, y);

            points.Add(point);
        }

        return points;
    }
}
