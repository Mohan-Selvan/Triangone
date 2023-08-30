using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BlockObjectPool blockPool = null;

    [Header("Settings")]
    [SerializeField] float blockScale = 1.0f;

    internal void Initialize()
    {
        blockPool.Initialize();
    }

    internal void Deinitialize()
    {

    }

    internal List<Block> GenerateLevel(List<Point> points)
    {
        List<Block> blocks = new List<Block>();

        //Triangulation
        List<Triangle> triangles = TriangulationManager.Triangulate(points);

        //Mesh setup for each triangle
        for (int i = 0; i < triangles.Count; i++)
        {
            Block block = blockPool.GetBlock();

            block.Setup(blockID: i, triangles[i]);

            block.transform.localPosition = triangles[i].GetCentroid();
            block.SetScale(blockScale);

            blocks.Add(block);
        }

        return blocks;
    }
}
