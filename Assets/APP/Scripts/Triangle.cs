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

        Vector2 SqrA = new Vector2(Mathf.Pow(a.x, 2f), Mathf.Pow(a.y, 2f)); 
        Vector2 SqrB = new Vector2(Mathf.Pow(b.x, 2f), Mathf.Pow(b.y, 2f)); 
        Vector2 SqrC = new Vector2(Mathf.Pow(c.x, 2f), Mathf.Pow(c.y, 2f));

        float D = ((a.x * (b.y - c.y)) + (b.x * (c.y - a.y)) + (c.x * (a.y - b.y))) * 2f;
        float x = (((SqrA.x + SqrA.y) * (b.y - c.y)) + ((SqrB.x + SqrB.y) * (c.y - a.y)) + ((SqrC.x + SqrC.y) * (a.y - b.y))) / D;
        float y = (((SqrA.x + SqrA.y) * (c.x - b.x)) + ((SqrB.x + SqrB.y) * (a.x - c.x)) + ((SqrC.x + SqrC.y) * (b.x - a.x))) / D;

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
