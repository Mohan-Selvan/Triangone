using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TriangulationManager
{
    public static List<Triangle> Triangulate(List<Point> points)
    {
        List<Triangle> triangles = new List<Triangle>();

        //Generating a super triangle..
        PointBounds pointBounds = GetPointBounds(points);
        Triangle superTriangle = GenerateSuperTriangle(pointBounds);
        triangles.Add(superTriangle);

        //Triangulation
        for(int p = 0; p < points.Count; p++)
        {
            Point point = points[p];

            //A list to contain all bad triangles
            List<Triangle> badTriangles = new List<Triangle>();

            //Finding all bad triangles
            for (int t = 0; t < triangles.Count; t++)
            {
                Triangle triangle = triangles[t];

                if (triangle.ContainsPointInCircumCircle(point))
                {
                    badTriangles.Add(triangle);
                }
            }

            //Creating new polygons
            List<Edge> polygonEdges = new List<Edge>();

            for(int bt = 0; bt < badTriangles.Count; bt++)
            {
                Triangle badTriangle = badTriangles[bt];
                List<Edge> edges = badTriangle.GetEdges();

                for (int e = 0; e < edges.Count; e++)
                {
                    Edge edge = edges[e];

                    bool rejectEdge = false;
                    for (int ot = 0; ot < badTriangles.Count; ot++)
                    {
                        Triangle otherTriangle = badTriangles[ot];
                        
                        if (ot != bt && otherTriangle.ContainsEdge(edge))
                        {
                            rejectEdge = true;
                        }
                    }

                    if (!rejectEdge)
                    {
                        polygonEdges.Add(edge);
                    }
                }
            }

            //Remove bad triangles
            for(int i = badTriangles.Count - 1; i >= 0; i--)
            {
                triangles.Remove(badTriangles[i]);
            }

            //Creating new triangles from polygonEdges
            for(int i = 0; i < polygonEdges.Count; i++)
            {
                Edge polygonEdge = polygonEdges[i];

                Point a = new Point(point.x, point.y);
                Point b = new Point(polygonEdge.PointA);
                Point c = new Point(polygonEdge.PointB);
                
                //Adding to triangles list.
                triangles.Add(new Triangle(a, b, c));
            }
        }

        //Remove super triangle.
        for(int t = triangles.Count - 1; t >= 0; t--)
        {
            List<Point> triangleVertices = triangles[t].GetPoints();
           
            for(int v = 0; v < triangleVertices.Count; v++)
            {
                Point vertex = triangleVertices[v];

                bool removeTriangle = false;

                //Check if any of the vertices match with the vertices of super triangle.
                foreach(Point superVertex in superTriangle.Vertices)
                {
                    if(vertex.EqualsPoint(superVertex))
                    {
                        removeTriangle = true;
                        break;
                    }
                }

                if (removeTriangle)
                {
                    triangles.RemoveAt(t);
                    break;
                }
            }
        }

        return triangles;
    }

    public static PointBounds GetPointBounds(List<Point> points)
    {
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;

        for(int i = 0; i < points.Count; i++)
        {
            Point point = points[i];

            if(minX > point.x) { minX = point.x; }
            if(maxX < point.x) { maxX = point.x; }
            if(minY > point.y) { minY = point.y; }
            if(maxY < point.y) { maxY = point.y; }
        }

        return new PointBounds(minX, maxX, minY, maxY);
    }

    public static Mesh CreateMeshFromTriangles(List<Triangle> trianglesLists)
    {
        Mesh mesh = new Mesh();

        int vertexCount = trianglesLists.Count * 3;

        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];
        int[] triangles = new int[vertexCount];


        int vertexIndex = 0;
        int triangleIndex = 0;

        for(int i = 0; i < trianglesLists.Count; i++)
        {
            Triangle delaunTriangle = trianglesLists[i];

            vertices[vertexIndex] = new Vector3(delaunTriangle.A.x, delaunTriangle.A.y, 0f);
            vertices[vertexIndex + 1] = new Vector3(delaunTriangle.B.x, delaunTriangle.B.y, 0f);
            vertices[vertexIndex + 2] = new Vector3(delaunTriangle.C.x, delaunTriangle.C.y, 0f);

            uvs[vertexIndex] = delaunTriangle.A.Position;
            uvs[vertexIndex + 1] = delaunTriangle.B.Position;
            uvs[vertexIndex + 2] = delaunTriangle.C.Position;

            triangles[triangleIndex] = vertexIndex + 2;
            triangles[triangleIndex + 1] = vertexIndex + 1;
            triangles[triangleIndex + 2] = vertexIndex + 0;

            vertexIndex += 3;
            triangleIndex += 3;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    public static Triangle GenerateSuperTriangle(PointBounds bounds)
    {
        float margin = 20f;

        float dMax = Mathf.Max(bounds.maxX - bounds.minX, bounds.maxY - bounds.minY) * margin;
        float xCen = (bounds.minX + bounds.maxX) * 0.5f;
        float yCen = (bounds.minY + bounds.maxY) * 0.5f;

        ///The float 0.866 is an arbitrary value determined for optimum supra triangle conditions.
        float x1 = xCen - 0.866f * dMax;
        float x2 = xCen + 0.866f * dMax;
        float x3 = xCen;

        float y1 = yCen - 0.5f * dMax;
        float y2 = yCen - 0.5f * dMax;
        float y3 = yCen + dMax;

        Point pointA = new Point(x1, y1);
        Point pointB = new Point(x2, y2);
        Point pointC = new Point(x3, y3);

        return new Triangle(pointA, pointB, pointC);
    }
}
