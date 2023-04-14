using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wnd_MessageTester : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Wnd_Message messageBox = null;

    [Header("Settings")]
    [SerializeField] string headingMessage = default;
    [SerializeField] string contentMessage = default;
    [SerializeField] bool enableCloseButton = true;
    //[SerializeField] List<ButtonOption> optionButtons = new List<ButtonOption>();

    [Header("Settings - Input")]
    [SerializeField] KeyCode showMessageKey = KeyCode.Alpha9;
    [SerializeField] KeyCode hideMessageKey = KeyCode.Alpha0;

    private void Update()
    {
        if(Input.GetKeyDown(showMessageKey))
        {
            messageBox.DisplayMessage(new Wnd_MessageParams()
            {
                HeadingMessage = headingMessage,
                ContentMessage = contentMessage,
                EnableCloseButton = true,
                ButtonOptions = new List<ButtonOption>()
                {
                    new ButtonOption
                    {
                        ButtonMessage = "Test button 1",
                        ButtonCallback = ()=> {Debug.Log("Test button 1"); }
                    },

                    new ButtonOption
                    {
                        ButtonMessage = "Test button 2",
                        ButtonCallback = ()=> {Debug.Log("Test button 2"); }
                    },
                }
            });

            //messageBox.DisplayMessage(new Wnd_MessageParams()
            //{
            //    HeadingMessage = headingMessage,
            //    ContentMessage = contentMessage,
            //    EnableCloseButton = true,
            //    ButtonOptions = optionButtons
            //});
        }
        else if (Input.GetKeyDown(hideMessageKey))
        {
            messageBox.HideWindow();
        }
    }
}
