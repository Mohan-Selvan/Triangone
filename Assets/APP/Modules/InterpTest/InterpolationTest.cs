using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InterpolationTest : MonoBehaviour
{
    [SerializeField] Transform targetObject = null;
    [SerializeField] Transform startPoint = null;
    [SerializeField] Transform endPoint = null;

    [SerializeField] float maxDuration = 3f;
    [SerializeField] AnimationCurve easingCurve = default;

    [Range(0f, 1f)]
    [SerializeField] float progress = default;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(StartLerpRoutine());
        }
    }

    private IEnumerator StartLerpRoutine()
    {
        Debug.Log("Lerp Routine : Start");

        float timer = 1f;

        while (timer > 0f)
        {
            timer -= (Time.deltaTime / maxDuration); // max duration = Number of seconds the animation should last

            float interpolationValue = 1f - Mathf.Clamp01(easingCurve.Evaluate(timer));

            targetObject.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, interpolationValue);

            progress = (1f- timer);

            yield return null;
        }

        progress = (1f - timer);

        Debug.Log("Lerp Routine : Stop");
    }
}
