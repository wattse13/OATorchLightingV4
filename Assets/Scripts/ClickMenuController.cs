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
    private TMP_Text myTitle;
    private TMP_Text myText;
    private Canvas clickMenuCanvas;
    private Canvas inspectMenuCanvas;
    private Canvas useMenuCanvas;

    public GameObject inspectButton;
    public GameObject useButton;
    public GameObject backButton;
    
    private void Awake()
    {
        
    }
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
        myTitle = GameObject.Find("EquipmentName").GetComponent<TMP_Text>();
        // myText = GameObject.Find("BodyText").GetComponent<TMP_Text>();
        clickMenuCanvas = GameObject.Find("ClickMenu").GetComponent<Canvas>();
        inspectMenuCanvas = GameObject.Find("InspectMenu").GetComponent<Canvas>();
        clickMenuCanvas.enabled = false;
        inspectMenuCanvas.enabled = false;
    }

    public void ShowClickMenu(GameObject myClickedPrefab)
    {
        // Show menu with equipment name and option to inspect or manipulate

        if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            Debug.Log("Clicked on equipment " + equipment.Name);
            
            // Should I set myClickedPrefab to a variable which can be used outside of this function?

            clickMenuCanvas.enabled = true; // Need way to close click menu
            myTitle.text = equipment.Name;
        }
    }

    public void OpenInspectMenu()
    {
        // Juice: Center object and blur out everything behind it
        // clickMenuCanvas.enabled = false;
        // inspectMenuCanvas.enabled = true;
        // myText = equipment.DescriptionUnsafe; // out of scope?
    }
}
