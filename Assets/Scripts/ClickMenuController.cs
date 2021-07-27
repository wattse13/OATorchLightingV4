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
    private TMP_Text myUseText;

    private GameObject clickMenu;
    private GameObject inspectMenu;
    private GameObject useMenu;
   
    // Buttons can probably all be private?
    public GameObject inspectButton;
    public GameObject backButton;
    public GameObject exitButton;

    public int offsetRight;
    public int offsetLeft;

    private RectTransform clickMenuTransform;

    // GameEvents is subscribed to this delegate event
    // Will be used to send a message when a GameObject's active status has been changed
    public delegate void EquipActivateEvent(GameObject e);
    public static event EquipActivateEvent OnActiveStatusChanged;

    private void OnEnable()
    {
        GameEvents.OnMessageSent += ShowClickMenu;
        GameEvents.OnMessageSent += MoveClickMenu;
    }

    private void OnDisable()
    {
        GameEvents.OnMessageSent -= ShowClickMenu;
        GameEvents.OnMessageSent -= MoveClickMenu;
    }

    private void Start()
    {
        // Uses a lot of string checks which probably isn't great
        clickMenu = GameObject.Find("ClickMenu");
        inspectMenu = GameObject.Find("InspectMenu");
        useMenu = GameObject.Find("UseMenu");

        myTitle = GameObject.Find("EquipName").GetComponent<TMP_Text>(); // Makes changes only in Click Menu
        myText = GameObject.Find("EquipDescription").GetComponent<TMP_Text>();
        myUseText = GameObject.Find("UseDescription").GetComponent<TMP_Text>();
        clickMenuTransform = clickMenu.GetComponent<RectTransform>();

        clickMenu.SetActive(false);
        inspectMenu.SetActive(false);
        useMenu.SetActive(false);
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
            myText.text = equipment.DescriptionUnsafe; // Should change based on equipment status
            myUseText.text = "Are you sure?"; // Meant as a check for now

            // Juice: Center clicked on GameObject and blur out everything else
        }
    }

    public void MoveClickMenu(GameObject myClickedPrefab)
    {
        if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            // I'm not doing my math correctly
            // Should use screen width and what not
            if (equipment.InitialPosition.x < 0)
            {
                clickMenuTransform.position = new Vector2(equipment.InitialPosition.x + offsetRight, 0);
            }
            else if (equipment.InitialPosition.x >= 0)
            {
                clickMenuTransform.position = new Vector2(equipment.InitialPosition.x - offsetLeft, 0);
            }
        }
    }
}
