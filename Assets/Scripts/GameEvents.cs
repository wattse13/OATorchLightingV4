using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to GameEvents GameObject
// Subscribed to OnClickDelegate
public class GameEvents : MonoBehaviour
{
    private EquipmentFactory equipmentFactory; // Does this create a dependency?
    // private EquipmentClass equipmentClass;

    // Delegates //

    // ClickMenuController is currently subscribed to this delegate event
    public delegate void MessageEvent(GameObject e);
    public static event MessageEvent OnMessageSent; // This works but feels hacky?

    private void Awake()
    {
        // equipmentClass = GetComponent<EquipmentClass>();
        equipmentFactory = GetComponent<EquipmentFactory>();
    }

    private void OnEnable()
    {
        OnClickDelegate.OnClicked += WhoCalled;
        // InspectMenuController.OnStatusChanged += myFunction;
        // ClickMenuController.OnActiveStatusChanged += myOtherFunction;
    }

    private void OnDisable()
    {
        OnClickDelegate.OnClicked -= WhoCalled;
        // InspectMenuController.OnStatusChanged -= myFunction;
        // ClickMenuController.OnActiveStatusChanged -= myOtherFunction;
    }

    // When GameEvents recieves a message from OnClickDelegate, it triggers its own delegate event, OnMessageSent
    // Passes clicked on GameObject as reference
    public void WhoCalled(GameObject myClickedPrefab)
    {
        // Triggers Message Event which alerts Click Menu Controller
        OnMessageSent?.Invoke(myClickedPrefab);
    }

    // Currently placeholder function
    public void myFunction(GameObject myClickedPrefab)
    {
        // Debug.Log("Hi myFunction");
        // This function should change boolean value of clicked on equipment safety status
    }

    // Currently placeholder function
    public void myOtherFunction(GameObject myClickedPrefab)
    {
        // Debug.Log("Hi myOtherFunction");
        // This function should change boolean value of clicked on equipment active status
    }
}
