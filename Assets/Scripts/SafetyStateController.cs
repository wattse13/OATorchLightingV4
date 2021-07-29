using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to SafetyStateController GameObject
public class SafetyStateController : MonoBehaviour
{
    Dictionary<int, bool> safetyStates = new Dictionary<int, bool>();
    private int currentID;

    public GameObject currentPrefab;

    // GameEvents is subscribed to this delegate event
    // SafetyStateController is subscribed to this delegate event
    // EquipmentClass is subscribed to this delegate event. Can't change values there, as it changes values for all GameObjects
    // Will be used to send a message when a GameObject's safety status has been changed
    public delegate void SafetyStatusEvent(GameObject e);
    public static event SafetyStatusEvent OnStatusChanged;
    // OnStatusChanged?.Invoke(this.gameObject);

    private void Awake()
    {
        // Integer key needs corresponds to GameObject ID
        // Very Important Key integer matches GameObject ID integer
        safetyStates.Add(1, false); // Corresponds to Cylinder 1
        safetyStates.Add(2, false); // Corresponds to Cylinder 2
    }

    private void OnEnable()
    {
        // When SafetyStatusEvent event is triggered, SetCurrentID function is called
        // Reference to clicked on GameObject is passed along with triggered event
        // InspectMenuController.OnStatusChanged += SetCurrentID;
        GameEvents.OnMessageSent += SetCurrentPrefab;
    }

    private void OnDisable()
    {
        // Unsubscribes from SafetyStatusEvent event if this script is disabled
        // InspectMenuController.OnStatusChanged -= SetCurrentID;
        GameEvents.OnMessageSent -= SetCurrentPrefab;
    }

    public void SetCurrentPrefab(GameObject myClickedPrefab)
    {
        currentPrefab = myClickedPrefab;
    }

    public GameObject GetCurrentPrefab()
    {
        return currentPrefab;
    }

    public void ReplaceButtonClicked()
    {
        OnStatusChanged?.Invoke(currentPrefab);
        // SetCurrentID(GameObject myClickedPrefab);
        // ChangeDictValue();
    }

    // This function finds the ID integer value of the passed in GameObject
    // The clicked on GameObject which has been passed with the SafetyStatusEvent event delegate is passed in myClickedPrefab argument
    private void SetCurrentID(GameObject myClickedPrefab)
    {
        // Determine GameObject ID
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            // Debug.Log(equipment.Name);
            currentID = equipment.ID;
        } // Should I add else statement to any of these?
        ChangeDictValue();
        ChangeSprite(myClickedPrefab); 
    }

    // Called after SetCurrentID finds the clicked on GameObject ID value
    // Clicked on GameObject ID value SHOULD correspond with specific key value in safetyStatus dictionary
    private void ChangeDictValue()
    {
        // Debug.Log(safetyStates[1]);
        safetyStates[currentID] = true;
        // Debug.Log(safetyStates[1]);
    }

    // Does it make sense to do that here? Changing it in EquipmentClass will change all GameObjects
    // Changes the clicked on GameObject sprite to safe variant
    private void ChangeSprite(GameObject myClickedPrefab)
    {
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            myClickedPrefab.GetComponent<SpriteRenderer>().sprite = equipment.SafeImage;
        }
    }
}
