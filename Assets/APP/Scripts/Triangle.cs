using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Triangle
{
    [SerializeField] Point[] vertices = default;

    public Point A { get => vertices[0]; }
    public Point B { get => vertices[1]; }
    public Point C { get => vertices[2]; }

    public Triangle(Point a, Point b, Point c)
    {
        vertices = new Point[3];

        bool isCounterClockwise = IsCounterClockwise(a, b, c);

        this.vertices[0] = a;
        this.vertices[1] = isCounterClockwise ? b : c;
        this.vertices[2] = isCounterClockwise ? c : b;
    }

    public Vector2 CalculateCircumCenter()
    {
        Vector2 a = A.Position;
        Vector2 b = B.Position;
        Vector2 c = C.Position;

        Vector2 a_sqr = new Vector2(Mathf.Pow(a.x, 2f), Mathf.Pow(a.y, 2f)); 
        Vector2 b_sqr = new Vector2(Mathf.Pow(b.x, 2f), Mathf.Pow(b.y, 2f)); 
        Vector2 c_sqr = new Vector2(Mathf.Pow(c.x, 2f), Mathf.Pow(c.y, 2f));

        float D = 2f * ((a.x * (b.y - c.y)) + (b.x * (c.y - a.y)) + (c.x * (a.y - b.y)));
        float x = ((a_sqr.x + a_sqr.y) * (b.y - c.y)) + ((b_sqr.x + b_sqr.y) * (c.y - a.y)) + ((c_sqr.x + c_sqr.y) * (a.y - b.y)) / D;
        float y = ((a_sqr.x + a_sqr.y) * (c.x - b.x)) + ((b_sqr.x + b_sqr.y) * (a.x - c.x)) + ((c_sqr.x + c_sqr.y) * (b.x - a.x)) / D;

        return new Vector2(x, y);
    }

    public float CalculateCircumRadius()
    {
        return Vector2.Distance(CalculateCircumCenter(), A.ToVector2());
    }

    /// <summary> Returns true if the provided points are in counter clockwise orientation, false if clockwise </summary>
    private bool IsCounterClockwise(Point pointA, Point pointB, Point pointC)
    {
        float result = ((pointB.x - pointA.x) * (pointC.y - pointA.y)) - ((pointC.x - pointA.x) * (pointB.y - pointA.y));
        return result > 0;
    }

    private void GetBisector(Vector2 pointA, Vector2 pointB)
    {
        Vector2 midPoint = (pointA + pointB) / 2f;

        float slope = (pointB.y - pointA.y) / (pointB.x - pointA.x);

        float perpendicularSlope = -1f / (slope);
    }
}
