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

    private GameObject clickMenu;
    private GameObject inspectMenu;
   
    public GameObject inspectButton;
    public GameObject useButton;
    public GameObject backButton;
    public GameObject replaceButton;
    
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
        // Uses a lot of string checks which probably isn't great
        clickMenu = GameObject.Find("ClickMenu");
        inspectMenu = GameObject.Find("InspectMenu");
        myTitle = GameObject.Find("EquipName").GetComponent<TMP_Text>(); // Makes changes only in Click Menu
        myText = GameObject.Find("EquipDescription").GetComponent<TMP_Text>();
        clickMenu.SetActive(false);
        inspectMenu.SetActive(false);
    }

    // This method is probably doing too many different things
    public void ShowClickMenu(GameObject myClickedPrefab)
    {
        // Show menu with equipment name and option to inspect or manipulate

        if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            Debug.Log("Clicked on equipment " + equipment.Name);

            clickMenu.SetActive(true); // Need way to close click menu and way to prevent re-clicking on object while inspect/use menu is open
            myTitle.text = equipment.Name;
            myText.text = equipment.DescriptionUnsafe;
        }
    }

    public void OpenInspectMenu()
    {
        // Juice: Center object and blur out everything behind it
        // clickMenu.enabled = false;
        // inspectMenuCanvas.enabled = true;
        // myText = equipment.DescriptionUnsafe; // out of scope?
    }
}
