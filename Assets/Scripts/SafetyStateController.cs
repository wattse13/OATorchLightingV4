using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to SafetyStateController GameObject
public class SafetyStateController : MonoBehaviour
{
    Dictionary<int, bool> safetyStates = new Dictionary<int, bool>();
    private int currentID;

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
        InspectMenuController.OnStatusChanged += SetCurrentID;
    }

    private void OnDisable()
    {
        // Unsubscribes from SafetyStatusEvent event if this script is disabled
        InspectMenuController.OnStatusChanged -= SetCurrentID;   
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
