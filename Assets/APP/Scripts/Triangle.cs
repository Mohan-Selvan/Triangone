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

        Vector2 a_sqr = new Vector2(Mathf.Pow(A.X, 2f), Mathf.Pow(A.Y, 2f)); 
        Vector2 b_sqr = new Vector2(Mathf.Pow(B.X, 2f), Mathf.Pow(B.Y, 2f)); 
        Vector2 c_sqr = new Vector2(Mathf.Pow(C.X, 2f), Mathf.Pow(C.Y, 2f));

        float D = 2f * ((A.X * (B.Y - C.Y)) + (B.X * (C.Y - A.Y)) + (C.X * (A.Y - B.Y)));
        float x = ((a_sqr.x + a_sqr.y) * (B.Y - C.Y)) + ((b_sqr.x + b_sqr.y) * (C.Y - A.Y)) + ((c_sqr.x + c_sqr.y) * (A.Y - B.Y)) / D;
        float y = ((a_sqr.x + a_sqr.y) * (C.X - B.X)) + ((b_sqr.x + b_sqr.y) * (A.X - C.X)) + ((c_sqr.x + c_sqr.y) * (B.X - A.X)) / D;

        return new Vector2(x, y);
    }

    public float CalculateCircumRadius()
    {
        return Vector2.Distance(CalculateCircumCenter(), A.ToVector2());
    }

    /// <summary> Returns true if the provided points are in counter clockwise orientation, false if clockwise </summary>
    private bool IsCounterClockwise(Point pointA, Point pointB, Point pointC)
    {
        float result = ((pointB.X - pointA.X) * (pointC.Y - pointA.Y)) - ((pointC.X - pointA.X) * (pointB.Y - pointA.Y));
        return result > 0;
    }

    private void GetCircumCenter(bool useBisector)
    {
        
    }

    private void GetBisector(Vector2 pointA, Vector2 pointB)
    {
        Vector2 midPoint = (pointA + pointB) / 2f;

        float slope = (pointB.y - pointA.y) / (pointB.x - pointA.x);

        float perpendicularSlope = -1f / (slope);
    }
}
