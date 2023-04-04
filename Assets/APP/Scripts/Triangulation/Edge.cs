using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    private Point _pointA;
    private Point _pointB;

    public Point PointA { get => _pointA; set => _pointA = value; }
    public Point PointB { get => _pointB; set => _pointB = value; }

    public Edge(Point pointA, Point pointB)
    {
        this._pointA = pointA;
        this._pointB = pointB;
    }

}
