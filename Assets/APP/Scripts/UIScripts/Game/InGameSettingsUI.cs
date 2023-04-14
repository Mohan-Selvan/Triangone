using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSettingsUI : MonoBehaviour
{
    [SerializeField] Button resumeButton = null;

    private void Start()
    {
        resumeButton.onClick.AddListener(HandleResumeButtonClicked);
    }

    private void HandleResumeButtonClicked()
    {
        GameWorld.Instance.GameManager.ResumeGame();
    }
}
