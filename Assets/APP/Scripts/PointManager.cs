using System.Collections;

using UnityEngine;

public class PointManager : MonoBehaviour
{
    private Coroutine _drawPointsRoutine = null;

    internal void Initialize()
    { 
    
    }

    internal void DrawPoints()
    {
        if(_drawPointsRoutine != null)
        {
            StopCoroutine(_drawPointsRoutine);
            _drawPointsRoutine = null;
        }

        _drawPointsRoutine = StartCoroutine(DrawPoints_Routine());
    }

    private IEnumerator DrawPoints_Routine()
    {


        yield return null;
    }
}
