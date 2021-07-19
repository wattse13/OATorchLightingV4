using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

// Attatched to the ClickMenuController GameObject
// Subscribed to the GameEvents OnMessageSent delegate event
// After GameEvents recieves message from instantiated objects, it should send a message to different controllers
public class ClickMenuController : MonoBehaviour
{
    private TMP_Text myText;
    private Canvas myCanvas;

    private void OnEnable()
    {
        GameEvents.OnMessageSent += ShowClickMenu;
    }

    private void OnDisable()
    {
        GameEvents.OnMessageSent -= ShowClickMenu;
    }

    private void Start()
    {
        myText = GameObject.Find("EquipmentName").GetComponent<TMP_Text>();

        myCanvas = GameObject.Find("ClickMenu").GetComponent<Canvas>();
        myCanvas.enabled = false;
    }

    public void ShowClickMenu(GameObject myClickedPrefab)
    {
        // Show menu with equipment name and option to inspect or manipulate

        Debug.Log("Click Menu Here!");
        Debug.Log(myClickedPrefab.name); // Can't figure out how to return specific object instance name
        myCanvas.enabled = true; // Need way to close click menu
        myText.text = myClickedPrefab.name;
    }
}
