using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Wnd_Message messageWindow = null;

    public Wnd_Message MessageWindow { get => messageWindow; }

    public void ShowMessageWindow(Wnd_MessageParams args)
    {
        MessageWindow.DisplayMessage(args);
    }

    public void CloseMessageWindow()
    {
        MessageWindow.HideWindow();
    }
}
