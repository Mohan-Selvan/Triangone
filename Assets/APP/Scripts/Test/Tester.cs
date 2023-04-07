using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Bounds bounds = default;

    [Header("Gizmo settings")]
    [SerializeField] Color gizmoColor = Color.white;
    [SerializeField] float gizmoRadius = 0.3f;

    private void OnDrawGizmosSelected()
    {
        if (bounds.size.sqrMagnitude <= 0.1f) { return; }

        Gizmos.color = gizmoColor;

        List<Point> points = Helpers.GetPointsOnBounds(bounds);

        for(int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawSphere(points[i].Position, gizmoRadius);
        }
    }
}
