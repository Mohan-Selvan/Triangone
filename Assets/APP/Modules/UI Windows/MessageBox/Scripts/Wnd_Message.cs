using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Wnd_MessageParams
{
    public string HeadingMessage = null;
    public string ContentMessage = null;

    public List<ButtonOption> ButtonOptions = null;
    public bool EnableCloseButton = true;
}

[System.Serializable]
public class ButtonOption
{
    public string ButtonMessage;
    public System.Action ButtonCallback;
}

[System.Serializable]
public class OptionButton
{
    public Button ButtonComponent;
    public TMP_Text ButtonText;
}

public class Wnd_Message : MonoBehaviour
{
    [Header("References - UI")]
    [SerializeField] GameObject container = null;
    [SerializeField] TMP_Text heading = null;
    [SerializeField] TMP_Text content = null;
    [SerializeField] Button closeButton = null;

    [Space(5)]
    [SerializeField] List<OptionButton> optionButtons = null;

    private void Start()
    {
        closeButton.onClick.AddListener(HideWindow);

        //Hiding window at the start here
        HideWindow();
    }

    public void DisplayMessage(Wnd_MessageParams args)
    {
        //Validation
        if(optionButtons.Count < args.ButtonOptions.Count)
        {
            Debug.LogError($"Only {optionButtons.Count} buttons are supported!");
            return;
        }

        ResetBox();

        heading.text = args.HeadingMessage;
        content.text = args.ContentMessage;

        for(int i = 0; i < args.ButtonOptions.Count; i++)
        {
            ConfigureButton(optionButtons[i], args.ButtonOptions[i]);
        }

        closeButton.gameObject.SetActive(args.EnableCloseButton);

        container.gameObject.SetActive(true);
    }

    public void HideWindow()
    {
        container.gameObject.SetActive(false);
    }

    private void ConfigureButton(OptionButton button, ButtonOption options)
    {
        button.ButtonText.text = options.ButtonMessage;

        button.ButtonComponent.onClick.RemoveAllListeners();
        button.ButtonComponent.onClick.AddListener(()=>{ 

            HideWindow();
            options.ButtonCallback?.Invoke();

        });

        button.ButtonComponent.gameObject.SetActive(true);
    }

    public void ResetBox()
    {
        for(int i = 0; i < optionButtons.Count; i++)
        {
            optionButtons[i].ButtonComponent.gameObject.SetActive(false);
        }
    }
}
