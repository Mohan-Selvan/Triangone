using System.Collections.Generic;
using UnityEngine;

public class TriangulationVisualizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TriangulationManager triangulator = null;
    [SerializeField] MeshRenderer meshRenderer = default;

    [Header("Debug")]
    [SerializeField] bool continuousRefresh = false;

    [Header("Settings")]
    [SerializeField] KeyCode triangulateKey = KeyCode.Alpha0;

    [Space(10)]
    [SerializeField] List<Point> points = new List<Point>();

    [Header("Gizmo settings")]
    [SerializeField] float gizmoSphereRadius = 0.05f;
    [SerializeField] Color gizmoLineColor = Color.yellow;
    [SerializeField] Color superTriangleLineColor = Color.magenta;
    [SerializeField] Color gizmoColor = Color.green;

    [Header("Testing only")]
    [SerializeField] List<Triangle> triangles = default;
    
    //Privates
    private MeshFilter _meshFilter = null;


    private void Start()
    {
        _meshFilter = meshRenderer.GetComponent<MeshFilter>();

        if (_meshFilter == null)
        {
            Debug.LogError("Could not find mesh filter..");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(triangulateKey) || continuousRefresh)
        {
            Debug.Log("Triangulating..");

            triangles = triangulator.Triangulate(points);
            _meshFilter.mesh = triangulator.CreateMeshFromTriangles(triangles);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) { return; }

        for(int i = 0; i < points.Count; i++)
        {
            Point point = points[i];

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(point.Position, gizmoSphereRadius);
        }

        PointBounds pointBounds = triangulator.GetPointBounds(points);
        Triangle superTriangle = TriangulationManager.GenerateSuperTriangle(pointBounds);

        Gizmos.color = superTriangleLineColor;
        DrawTriangle(superTriangle);

        //Debug.Log($"Super triangle : {superTriangle.A.Position}, {superTriangle.B.Position}, {superTriangle.C.Position}");

        if (triangles != null && triangles.Count > 0)
        {
            Gizmos.color = gizmoLineColor;

            for (int i = 0; i < triangles.Count; i++)
            {
                DrawTriangle(triangles[i]);
            }
        }

        void DrawTriangle(Triangle triangle)
        {
            Gizmos.DrawLine(triangle.A.Position, triangle.B.Position);
            Gizmos.DrawLine(triangle.B.Position, triangle.C.Position);
            Gizmos.DrawLine(triangle.C.Position, triangle.A.Position);
        }
    }
}
