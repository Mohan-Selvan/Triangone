using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Triangle triangle = null;
    [SerializeField] MeshFilter meshFilter = null;
    [SerializeField] Transform containerTransform = null;

    [Header("References - UI")]
    [SerializeField] TMP_Text areaText = null;

    [Header("Testing only")]
    [SerializeField] Mesh mesh = null;
    [SerializeField] private MeshRenderer _meshRenderer = null;

    private void Awake()
    {
        _meshRenderer = meshFilter.GetComponent<MeshRenderer>();
    }

    internal void Setup(Triangle _triangle)
    {
        if(mesh == null)
        {
            Mesh meshInstance = new Mesh();
            meshInstance.name = "Block";
            this.mesh = meshInstance;

            //Applying mesh
            meshFilter.mesh = this.mesh;
        }

        this.triangle = _triangle;

        //Resetting position
        this.transform.position = Vector3.zero;
        containerTransform.localPosition = Vector3.zero;

        //Offsetting points according to centroid
        Vector2 centroid = triangle.GetCentroid();
        Vector2 a = (Vector2)containerTransform.position + ((triangle.A.Position - centroid).normalized * (triangle.A.Position - centroid).magnitude);
        Vector2 b = (Vector2)containerTransform.position + ((triangle.B.Position - centroid).normalized * (triangle.B.Position - centroid).magnitude);
        Vector2 c = (Vector2)containerTransform.position + ((triangle.C.Position - centroid).normalized * (triangle.C.Position - centroid).magnitude);

        UpdateMesh(a, b, c);

        //Updating area to UI.
        this.areaText.text = $"{triangle.GetArea()}";
    }

    internal void SetScale(float scale)
    {
        this.containerTransform.localScale = scale * Vector3.one;
    }

    private void UpdateMesh(Vector2 posA, Vector2 posB, Vector2 posC)
    {
        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];

        int vertexIndex = 0;
        int triangleIndex = 0;

        vertices[vertexIndex + 0] = new Vector3(posA.x, posA.y, 0f);
        vertices[vertexIndex + 1] = new Vector3(posB.x, posB.y, 0f);
        vertices[vertexIndex + 2] = new Vector3(posC.x, posC.y, 0f);


        uv[vertexIndex + 0] = posA;
        uv[vertexIndex + 1] = posB;
        uv[vertexIndex + 2] = posC;


        triangles[triangleIndex + 0] = vertexIndex + 2;
        triangles[triangleIndex + 1] = vertexIndex + 1;
        triangles[triangleIndex + 2] = vertexIndex + 0;

        this.mesh.vertices = vertices;
        this.mesh.uv = uv;
        this.mesh.triangles = triangles;

        this.mesh.RecalculateNormals();
    }
}
