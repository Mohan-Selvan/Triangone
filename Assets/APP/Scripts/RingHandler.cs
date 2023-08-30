using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera screenCamera = null;

    [Space(10)]
    [SerializeField] List<Wall> walls = default;

    [Header("Settings")]
    [SerializeField] float outwardOffset = 100f;
    [SerializeField] float inwardOffset = 0.3f;

    private List<Wall> randomizedWalls = default;

    internal void Initialize()
    {
        SetupRing();

        //Caching randomized walls
        randomizedWalls = new List<Wall>();

        for(int i = 0; i < walls.Count; i++)
        {
            randomizedWalls.Add(walls[i]);
        }
    }

    internal void Deinitialize()
    {

    }

    public int WallCount => walls.Count;

    private void SetupRing()
    {
        Debug.Log("Setting up ring..");
        
        Vector2 screenTL = screenCamera.ViewportToWorldPoint(new Vector3(0, 1));
        Vector2 screenTR = screenCamera.ViewportToWorldPoint(new Vector3(1, 1));
        Vector2 screenBL = screenCamera.ViewportToWorldPoint(new Vector3(0, 0));
        Vector2 screenBR = screenCamera.ViewportToWorldPoint(new Vector3(1, 0));

        int i = 0;

        // Wall - Left
        {
            Vector2 tl = screenTL + new Vector2(-1, 1) * outwardOffset;
            Vector2 tr = screenTL + new Vector2( 1,-1) * inwardOffset;
            Vector2 bl = screenBL + new Vector2(-1,-1) * outwardOffset;
            Vector2 br = screenBL + new Vector2( 1, 1) * inwardOffset;

            SetupWall(walls[i], tl, tr, br, bl);
        }

        i++;

        // Wall - Top
        {
            Vector2 tl = screenTL + new Vector2(-1, 1) * outwardOffset;
            Vector2 tr = screenTR + new Vector2( 1, 1) * outwardOffset;
            Vector2 bl = screenTL + new Vector2( 1,-1) * inwardOffset;
            Vector2 br = screenTR + new Vector2(-1,-1) * inwardOffset;

            SetupWall(walls[i], tl, tr, br, bl);
        }

        i++;

        // Wall - Bottom
        {
            Vector2 tl = screenBL + new Vector2( 1, 1) * inwardOffset;
            Vector2 tr = screenBR + new Vector2(-1, 1) * inwardOffset;
            Vector2 bl = screenBL + new Vector2(-1,-1) * outwardOffset;
            Vector2 br = screenBR + new Vector2( 1,-1) * outwardOffset;

            SetupWall(walls[i], tl, tr, br, bl);
        }

        i++;

        // Wall - Right
        {
            Vector2 tl = screenTR + new Vector2(-1,-1) * inwardOffset;
            Vector2 tr = screenTR + new Vector2( 1, 1) * outwardOffset;
            Vector2 bl = screenBR + new Vector2(-1, 1) * inwardOffset;
            Vector2 br = screenBR + new Vector2( 1,-1) * outwardOffset;

            SetupWall(walls[i], tl, tr, br, bl);
        }

        Debug.Log("Ring setup complete");
    }

    internal void SetWallSafeState_All(bool isSafe, bool animate)
    {
        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].SetWallSafeState(isSafe, animate: animate);
        }
    }

    internal void RandomizeWallSafeStates(int numberOfUnsafeWalls)
    {
        SetWallSafeState_All(isSafe: true, animate: false);

        if(numberOfUnsafeWalls > walls.Count)
        {
            Debug.LogError($"NumberOfUnsafeWalls {numberOfUnsafeWalls} must be lesser than wall count {walls.Count}");
            return;
        }

        //Shuffling randomized walls
        for(int i = 0; i < randomizedWalls.Count; i++)
        {
            int randomIndex = Random.Range(0, randomizedWalls.Count);

            if(randomIndex == i) { continue; }

            Wall temp = randomizedWalls[i];
            randomizedWalls[i] = randomizedWalls[randomIndex];
            randomizedWalls[randomIndex] = temp;
        }

        for(int i = 0; i < randomizedWalls.Count; i++)
        {
            bool safe = (i < numberOfUnsafeWalls);
            randomizedWalls[i].SetWallSafeState(safe);
        }
    }

    private void SetupWall(Wall wall, Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft)
    {   
        Vector2[] vertices = new Vector2[] {
            topLeft,
            topRight,
            bottomRight,
            bottomLeft
        };

        Mesh mesh = wall.GetMesh();
        SetupMesh(mesh, vertices);

        PolygonCollider2D collider = wall.GetPolygonCollider2D();

        collider.points = vertices;
    }

    private void SetupMesh(Mesh mesh, Vector2[] points)
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        int vertexIndex = 0;
        int triangleIndex = 0;

        vertices[vertexIndex + 0] = points[0];
        vertices[vertexIndex + 1] = points[1];
        vertices[vertexIndex + 2] = points[2];
        vertices[vertexIndex + 3] = points[3];


        uv[vertexIndex + 0] = points[0];
        uv[vertexIndex + 1] = points[1];
        uv[vertexIndex + 2] = points[2];
        uv[vertexIndex + 3] = points[3];


        triangles[triangleIndex + 0] = vertexIndex + 0;
        triangles[triangleIndex + 1] = vertexIndex + 2;
        triangles[triangleIndex + 2] = vertexIndex + 3;
        triangles[triangleIndex + 3] = vertexIndex + 2;
        triangles[triangleIndex + 4] = vertexIndex + 0;
        triangles[triangleIndex + 5] = vertexIndex + 1;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
