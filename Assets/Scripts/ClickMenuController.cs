using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to the ClickMenuController GameObject
// Subscribed to the GameEvents OnMessageSent delegate event
// After GameEvents recieves message from instantiated objects, it should send a message to different controllers
public class ClickMenuController : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnMessageSent += ShowClickMenu;
    }

    private void OnDisable()
    {
        GameEvents.OnMessageSent -= ShowClickMenu;
    }
    public void ShowClickMenu(GameObject myClickedPrefab)
    {
        // Show menu with equipment name and option to inspect or manipulate

        Debug.Log("Click Menu Here!");
        Debug.Log(myClickedPrefab.name); // Can't figure out how to return specific object instance name
    }
}
