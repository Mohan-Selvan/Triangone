using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static List<Point> GetPointsOnBounds(Bounds bound, float intervalRateNormalized)
    {
        Vector2 topLeft = bound.center + new Vector3(-bound.extents.x, bound.extents.y, 0f);
        Vector2 topRight = bound.center + new Vector3(bound.extents.x, bound.extents.y, 0f);
        Vector2 bottomLeft = bound.center + new Vector3(-bound.extents.x, -bound.extents.y, 0f);
        Vector2 bottomRight = bound.center + new Vector3(bound.extents.x, -bound.extents.y, 0f);

        List<Point> result = new List<Point>()
        {
            new Point(topLeft),
            new Point(topRight),
            new Point(bottomLeft),
            new Point(bottomRight)
        };


        AddPointsBetween(topLeft, topRight, ref result);
        AddPointsBetween(bottomLeft, bottomRight, ref result);
        AddPointsBetween(topLeft, bottomLeft, ref result);
        AddPointsBetween(topRight, bottomRight, ref result);

        return result;

        void AddPointsBetween(Vector2 a, Vector2 b, ref List<Point> points)
        {
            float intervalT = intervalRateNormalized;
            int count = (int)(1f / intervalT);

            for (int i = 1; i <= count; i++)
            {
                float t = (float)i * intervalT;

                Vector2 targetPosition = Vector2.Lerp(a, b, t);

                Point p = new Point(targetPosition);
                points.Add(p);
            }
        }
    }
}
