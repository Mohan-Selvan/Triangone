using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleDebugger : MonoBehaviour
{
    [SerializeField] List<Point> points = default;

    [Header("Settings")]
    [SerializeField] bool enableContinuousRefresh = false;
    [SerializeField] bool enableGizmos = false;

    [Header("Gizmo Seettings")]
    [SerializeField] Color lineColor = Color.white;
    [SerializeField] Color circleColor = Color.white;

    [Header("Testing only")]
    [SerializeField] Triangle triangle = null;

    private void Start()
    {
        if (points.Count != 3)
        {
            Debug.LogError($"3 Points are needed for Triangle, Current number of points : {points.Count}");
        }

        triangle = new Triangle(points[0], points[1], points[2]);
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) { return; }

        if (!enableGizmos) { return; }

        if(triangle == null) { return; }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(triangle.A.ToVector2(), triangle.B.ToVector2());

        Gizmos.color = Color.green;
        Gizmos.DrawLine(triangle.B.ToVector2(), triangle.C.ToVector2());

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(triangle.C.ToVector2(), triangle.A.ToVector2());


        Gizmos.color = circleColor;
        Vector2 circumCenter =  triangle.CalculateCircumCenter(); //Utils.FindCircumCenter(triangle.A.Position, triangle.B.Position, triangle.C.Position);
        float circumRadius = Vector2.Distance(circumCenter, triangle.A.Position);

        Gizmos.DrawWireSphere(circumCenter, circumRadius);

        Gizmos.color = lineColor;
        //Gizmos.DrawLine(circumCenter, triangle.A.ToVector2());
        //Gizmos.DrawLine(circumCenter, triangle.B.ToVector2());
        //Gizmos.DrawLine(circumCenter, triangle.C.ToVector2());
    }

    #endregion
}
