using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("References - InGame")]
    [SerializeField] Button pauseButton = null;

    [Header("References - Settings")]
    [SerializeField] GameObject inGameSettingsContainer = null;
    [SerializeField] Button resumeButton = null;
    [SerializeField] Button quitButton = null;

    //Helpers
    GameManager GameManager => GameWorld.Instance.GameManager;
    UIManager UIManager => GameWorld.Instance.UIManager;

    private void Start()
    {
        pauseButton.onClick.AddListener(HandlePauseButtonClicked);
        resumeButton.onClick.AddListener(HandleResumeButtonClicked);
        quitButton.onClick.AddListener(HandleQuitButonClicked);
    }

    private void HandleQuitButonClicked()
    {
        //Local function
        void QuitGame()
        {
            //GameManager.EndGame();
        }

        //Disabling settings menu
        EnableSettingsOverlay(false);

        //Confirmation box
        UIManager.ShowMessageWindow(new Wnd_MessageParams()
        {
            HeadingMessage = "Confirmation",
            ContentMessage = "Are you sure you want to quit?",
            EnableCloseButton = true,
            ButtonOptions = new System.Collections.Generic.List<ButtonOption>()
            {
                new ButtonOption()
                {
                    ButtonMessage = "Yes",
                    ButtonCallback = () => {
                        QuitGame();
                    }
                },

                new ButtonOption()
                {
                    ButtonMessage = "No",
                    ButtonCallback = null
                }
            }
        });
    }

    private void HandlePauseButtonClicked()
    {
        //GameManager.PauseGame();
        EnableSettingsOverlay(true);
    }

    private void HandleResumeButtonClicked()
    {
        //GameManager.ResumeGame();
        EnableSettingsOverlay(false);
    }

    private void EnableSettingsOverlay(bool value)
    {
        inGameSettingsContainer.gameObject.SetActive(value);
    }
}
