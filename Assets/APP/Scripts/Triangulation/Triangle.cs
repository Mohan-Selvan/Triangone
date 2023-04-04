using System;
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
    public Point[] Vertices { get => vertices; private set => vertices = value; }

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

    internal List<Edge> GetEdges()
    {
        return new List<Edge>()
        {
            new Edge(A, B),
            new Edge(B, C),
            new Edge(A, C)
        };
    }

    internal List<Point> GetPoints()
    {
        return new List<Point>()
        {
            A, B, C 
        };
    }

    internal float GetArea()
    {
        // (((b - a) X (c - a)).length) / 2f // X => Cross product

        return 0.5f * Vector3.Cross((B.Position - A.Position), (C.Position - A.Position)).magnitude;
    }

    internal Vector2 GetCentroid()
    {
        float x = (A.Position.x + B.Position.x + C.Position.x) / 3f;
        float y = (A.Position.y + B.Position.y + C.Position.y) / 3f;

        return new Vector2(x, y);
    }

    #region Helpers

    public bool ContainsPointInCircumCircle(Point point)
    {
        float distance = Vector2.Distance(point.Position, CalculateCircumCenter());
        return  distance < CalculateCircumRadius();
    }

    public bool ContainsEdge(Edge edge)
    {
        int commonPoints = 0;

        foreach (Point point in vertices)
        {
            if (point.EqualsPoint(edge.PointA) || point.EqualsPoint(edge.PointB))
            {
                commonPoints++;
            }
        }

        return (commonPoints >= 2);
    }

    #endregion
}
