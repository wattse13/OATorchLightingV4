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
    private TMP_Text myUseText;

    private GameObject clickMenu;
    private GameObject inspectMenu;
    private GameObject useMenu;

    public int offsetRight;
    public int offsetLeft;

    private RectTransform clickMenuTransform;

    // EquipmentClass is subscribed to this delegate event (disables colliders when Inspect/Use menu is open)
    // Invoked when Click Menu is first opened
    public delegate void ClickMenuEvent();
    public static event ClickMenuEvent OnClickMenu;
    // public static event ClickMenuEvent OnClickMenuDisable;

    // EquipmentController is subscribed to this delegate event (disables colliders when Inspect/Use menu is open)
    // Invoked when ClickMenu transitions over to Inspect/UseMenu
    public delegate void InspectButtonEvent();
    public static event InspectButtonEvent OnInspectClicked;

    private void OnEnable()
    {
        OnClickDelegate.OnClicked += ShowClickMenu;
        OnClickDelegate.OnClicked += MoveClickMenu;
    }

    private void OnDisable()
    {
        OnClickDelegate.OnClicked -= ShowClickMenu;
        OnClickDelegate.OnClicked -= MoveClickMenu;
    }

    private void Start()
    {
        // Uses a lot of string checks which probably isn't great
        clickMenu = GameObject.Find("ClickMenu");
        inspectMenu = GameObject.Find("InspectMenu");
        useMenu = GameObject.Find("UseMenu");

        myTitle = GameObject.Find("EquipName").GetComponent<TMP_Text>(); // Makes changes only in Click Menu
        myUseText = GameObject.Find("UseDescription").GetComponent<TMP_Text>();
        clickMenuTransform = clickMenu.GetComponent<RectTransform>();

        // Should Inspect and Use menus bet SetActive() in their own scripts?
        clickMenu.SetActive(false);
        inspectMenu.SetActive(false);
        useMenu.SetActive(false);
    }

    // This method is probably doing too many different things
    // Show menu with equipment name and option to inspect or manipulate
    public void ShowClickMenu(GameObject myClickedPrefab)
    {
        if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            clickMenu.SetActive(true);
            myTitle.text = equipment.Name;
        }
        OnClickMenu?.Invoke();
    }

    // Makes sure that the ClickMenu does not display off screen
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

    // Triggers DisableColliders() in the EquipmentClass 
    public void InspectButtonClick()
    {
        OnInspectClicked?.Invoke();
    }
}
