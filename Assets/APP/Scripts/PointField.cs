using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointField : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BlockObjectPool blockPool = null;
    [SerializeField] SelectionManager selectionManager = null;

    [Header("Debug only")]
    [SerializeField] bool continuousRefresh = false;

    [Header("Settings")]
    [SerializeField] int numberOfPointsToGenerate = 100;
    [SerializeField] float blockScale = 1.0f;
    [SerializeField] Bounds fieldBounds;

    [Range(0.1f, 0.9f)]
    [SerializeField] float boundsInterval = 0.2f;

    [Header("Settigs - Input")]
    [SerializeField] KeyCode drawLevelKey = KeyCode.Alpha0;
    [SerializeField] KeyCode updateScaleKey = KeyCode.Alpha9;

    [Header("Settigs - Gizmos")]
    [SerializeField] Color gizmo_BoundsColor = Color.green;

    [Space(5)]
    [SerializeField] float gizmo_PointRadius = 0.1f;
    [SerializeField] Color gizmo_PointColor = Color.yellow;

    [Space(10)]
    [SerializeField] float gizmo_CentroidRadius = 0.1f;
    [SerializeField] Color gizmo_CentroidColor = Color.magenta;

    [Space(10)]
    [SerializeField] float gizmo_PositionRadius = 0.1f;
    [SerializeField] Color gizmo_PositionColor = Color.red;

    [Header("Testing only")]
    [SerializeField] List<Point> _points = null;
    [SerializeField] List<Triangle> _triangles = null;

    private List<Block> blocks = null;

    private void Start()
    {
        blocks = new List<Block>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(drawLevelKey))
        {
            Debug.Log($"{nameof(PointField)} : {nameof(DrawLevel_Routine)}");
            StartCoroutine(DrawLevel_Routine());
        }

        if (Input.GetKeyDown(updateScaleKey) || continuousRefresh)
        {
            Debug.Log($"{nameof(PointField)} : {nameof(UpdateScale)}");
            UpdateScale();
        }
    }

    public IEnumerator DrawLevel_Routine()
    {
        //Returning all existing blocks
        for (int i = 0; i < blocks.Count; i++)
        {
            blockPool.ReturnBlock(blocks[i]);
        }

        blocks.Clear();

        //_points = new List<Point>();
        _points = Helpers.GetPointsOnBounds(fieldBounds, intervalRateNormalized: boundsInterval);

        //Creating point field
        for(int i = 0; i < numberOfPointsToGenerate; i++)
        {
            //TODO :: Add slight offset here.
            float x = Random.Range(fieldBounds.min.x, fieldBounds.max.x);
            float y = Random.Range(fieldBounds.min.y, fieldBounds.max.y);

            Point point = new Point(x, y);

            _points.Add(point);
        }

        //Triangulation
        _triangles = TriangulationManager.Triangulate(_points);
        
        //Mesh setup for each triangle
        for(int i = 0; i < _triangles.Count; i++)
        {
            Block block = blockPool.GetBlock();

            block.Setup(blockID: i, _triangles[i]);


            block.transform.localPosition = _triangles[i].GetCentroid();
            block.SetScale(blockScale);

            blocks.Add(block);
        }

        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].EnableBlock(value: true, animate: true, delay: Random.Range(0.1f, 1f));
        }

        yield return new WaitForSeconds(2f);

        selectionManager.UpdateBlocks(blocks);
        selectionManager.DeselectAllBlocks();
    }

    public void UpdateScale()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].SetScale(blockScale);
        }
    }

    private List<Point> GetPointsOnBounds(Bounds bound)
    {
        Vector2 topLeft = bound.center - new Vector3(bound.min.x, bound.max.y, 0f);
        Vector2 topRight = bound.center - new Vector3(bound.max.x, bound.max.y, 0f);
        Vector2 bottomLeft = bound.center - new Vector3(bound.min.x, bound.min.y, 0f);
        Vector2 bottomRight = bound.center - new Vector3(bound.min.x, bound.max.y, 0f);

        List<Point> result = new List<Point>()
        {
            new Point(topLeft),
            new Point(topRight),
            new Point(bottomLeft),
            new Point(bottomRight)
        };

        float intervalNormalized = 0.1f;

        AddPointsBetween(topLeft, topRight, ref result);
        AddPointsBetween(topRight, bottomRight, ref result);
        AddPointsBetween(bottomRight, bottomLeft, ref result);
        AddPointsBetween(bottomLeft, topLeft, ref result);

        return result;
        
        void AddPointsBetween(Vector2 a, Vector2 b, ref List<Point> points)
        {
            float distance = Vector2.Distance(a, b);
            Vector2 direction = (b - a).normalized;
            int intervalDistance = (int)(distance * intervalNormalized);
            int count = (int)(1f / intervalNormalized);

            for (int i = 1; i < count; i++)
            {
                Point p = new Point(a + (direction * (i * intervalDistance)));
                points.Add(p);
            }
        }
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmo_BoundsColor;
        Gizmos.DrawWireCube(fieldBounds.center, fieldBounds.size);

        if (Application.isPlaying)
        {
            Gizmos.color = gizmo_PointColor;

            for (int i = 0; i < _points.Count; i++)
            {
                Gizmos.DrawSphere(_points[i].Position, gizmo_PointRadius);
            }

            if(_triangles != null)
            {
                Gizmos.color = gizmo_CentroidColor;

                for (int i = 0; i < _triangles.Count; i++)
                {
                    Gizmos.DrawSphere(_triangles[i].GetCentroid(), gizmo_CentroidRadius);
                }
            }

            if(blocks != null)
            {
                Gizmos.color = gizmo_PositionColor;

                for (int i = 0; i < blocks.Count; i++)
                {
                    Gizmos.DrawSphere(blocks[i].transform.position, gizmo_PositionRadius);
                }
            }

        }
    }

    #endregion

    #region Obselete

    private void ScaleAroundCentroid()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            Vector3 A = blocks[i].transform.position;
            Vector3 B = _triangles[i].GetCentroid();

            Vector3 C = A - B; // diff from object pivot to desired pivot/origin

            Vector3 targetScale = blockScale * Vector3.one;

            float RS = targetScale.x; //targetScale.x / blocks[i].transform.localScale.x; // relataive scale factor

            // calc final position post-scale
            Vector3 FP = B + C * RS;

            // finally, actually perform the scale/translation
            blocks[i].transform.localScale = targetScale;
            blocks[i].transform.localPosition = FP;
        }
    }


    #endregion
}
