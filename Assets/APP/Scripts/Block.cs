using UnityEngine;

using TMPro;
using Pixelplacement.TweenSystem;
using Pixelplacement;
using System;

public class Block : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Triangle triangle = null;
    [SerializeField] MeshFilter meshFilter = null;
    [SerializeField] Transform containerTransform = null;
    [SerializeField] PolygonCollider2D polygonCollider = null;

    [Header("References - UI")]
    [SerializeField] TMP_Text areaText = null;

    [Header("Testing only")]
    [SerializeField] int blockID = 0;
    [SerializeField] Mesh mesh = null;
    [SerializeField] private MeshRenderer _meshRenderer = null;

    //Properties
    public int BlockID { get => blockID; set => blockID = value; }

    private void Awake()
    {
        _meshRenderer = meshFilter.GetComponent<MeshRenderer>();
    }

    internal void Setup(int blockID, Triangle _triangle)
    {
        if(mesh == null)
        {
            Mesh meshInstance = new Mesh();
            meshInstance.name = "Block";
            this.mesh = meshInstance;

            //Applying mesh
            meshFilter.mesh = this.mesh;
        }

        this.blockID = blockID;
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

        polygonCollider.points = new Vector2[]
        {
            a, b, c
        };

        //Updating area to UI.
        //this.areaText.text = $"{triangle.GetArea()}";

        _meshRenderer.material.SetColor("_BaseColor", GameSettings.Instance.BlockDefaultColor);

        containerTransform.gameObject.SetActive(false);
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
        this.mesh.RecalculateBounds();
    }

    public void EnableBlock(bool value, bool animate, float delay = 0f)
    {
        if(animate)
        {
            //If animation is required.

            Color currentColor = _meshRenderer.material.color;

            Color fromColor = Utils.GetColorWithAlpha(currentColor, 0.0f);
            Color toColor = Utils.GetColorWithAlpha(currentColor, 1.0f);

            if (!value)
            {
                fromColor = Utils.GetColorWithAlpha(currentColor, 1.0f);
                toColor = Utils.GetColorWithAlpha(currentColor, 0.0f);
            }

            TweenBase tween = Tween.ShaderColor(_meshRenderer.material, "_BaseColor", startValue: fromColor,
                endValue: toColor, 
                duration: 0.15f,
                delay: delay, 
                easeCurve: Tween.EaseInOut,
                startCallback: () =>
                {
                    polygonCollider.enabled = value;
                    _meshRenderer.material.color = fromColor;
                    containerTransform.gameObject.SetActive(true);
                },
                
                completeCallback: ()=>
                {
                    try
                    {
                        polygonCollider.enabled = value;
                        _meshRenderer.material.color = toColor;
                        containerTransform.gameObject.SetActive(value);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError($"Exception on tween callback : {e}");
                    }

                });
        }
        else
        {
            //If animation is not required.
            _meshRenderer.material.color = Utils.GetColorWithAlpha(_meshRenderer.material.color, 1f);
            this.containerTransform.gameObject.SetActive(value);
        }
    }

    public bool IsBlockColliding(LayerMask layerMask)
    {
        return polygonCollider.IsTouchingLayers(layerMask);
    }

    internal void HandleBlockTouchedRing()
    {
        Debug.Log($"Removing block : {blockID}");
        EnableBlock(value: false, animate: true);
    }

    #region Selection handlers

    public void HandleSelectionStateChanged(bool isSelected)
    {
        Color targetColor = isSelected ? GameSettings.Instance.BlockHighlightColor : GameSettings.Instance.BlockDefaultColor;
        _meshRenderer.material.SetColor("_BaseColor", targetColor);
    }

    #endregion
}
