using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskLoaderUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject container = null;
    [SerializeField] TMP_Text headingText = null;

    [SerializeField] Slider progressSlider = null;

    private void Start()
    {
        progressSlider.interactable = false;

        progressSlider.maxValue = 1f;
        progressSlider.minValue = 0f;
    }

    internal void ShowLoaderUI(string headingMessage, bool trackProgress)
    {
        headingText.text = headingMessage;

        //Progress type
        progressSlider.gameObject.SetActive(trackProgress);

        container.SetActive(true);
    }

    internal void DisableLoaderUI()
    {
        container.gameObject.SetActive(false);
    }

    internal void UpdateProgress(float progressNormalized)
    {
        this.progressSlider.SetValueWithoutNotify(progressNormalized);
    }
}
