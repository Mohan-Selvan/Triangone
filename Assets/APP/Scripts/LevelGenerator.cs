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

    public List<Block> GenerateLevel(LevelData levelData)
    {
        List<Triangle> triangles = new List<Triangle>();
        List<Block> blocks = new List<Block>();

        foreach (Shape s in levelData.Shapes)
        {
            foreach(Poly p in s.PolyList)
            {
                if(p.Coords.Count != 3)
                {
                    Debug.LogError("Invalid level data");
                    return blocks;
                }

                Triangle triangle = new Triangle(
                    new Point(p.Coords[0].ToVector2()),
                    new Point(p.Coords[1].ToVector2()),
                    new Point(p.Coords[2].ToVector2()));

                triangles.Add(triangle);
            }
        }

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
