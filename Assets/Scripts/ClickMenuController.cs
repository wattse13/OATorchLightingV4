using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMenuController : MonoBehaviour
{
    // After GameEvents recieves message from instantiated objects, it should send a message to different controllers

    private void OnEnable()
    {
        GameEvents.OnMessageSent += ShowClickMenu;
    }

    private void OnDisable()
    {
        GameEvents.OnMessageSent -= ShowClickMenu;
    }
    public void ShowClickMenu()
    {
        // Show menu with equipment name and option to inspect or manipulate

        Debug.Log("Click Menu Here!");
    }
}
