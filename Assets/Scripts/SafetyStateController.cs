using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to SafetyStateController GameObject
public class SafetyStateController : MonoBehaviour
{
    Dictionary<int, bool> safetyStates = new Dictionary<int, bool>();

    private int currentID;
    private int safeEquipCount = 0;

    private bool isAllSafe = false;

    // public GameObject currentPrefab; // Not currently needed

    // InspectMenuController is subscribed to this delegate event
    // Will be used to send a message when a GameObject's safety status has been changed
    public delegate void SafetyStatusEvent();
    public static event SafetyStatusEvent OnStatusChanged;
    // OnStatusChanged?.Invoke(this.gameObject);

    private void Awake()
    {
        // Integer key must correspond to GameObject ID
        // Very Important Key integer matches GameObject ID integer
        safetyStates.Add(1, false); // Corresponds to Cylinder 1
        safetyStates.Add(2, false); // Corresponds to Cylinder 2
    }

    private void OnEnable()
    {
        EquipmentController.OnValueChanged += SetCurrentID;
        // GameEvents.OnMessageSent += SetCurrentPrefab; // Not currently needed
    }

    private void OnDisable()
    {
        EquipmentController.OnValueChanged -= SetCurrentID;
        // GameEvents.OnMessageSent -= SetCurrentPrefab; // Not currently needed
    }

    // This function finds the ID integer value of the passed in GameObject
    // The clicked on GameObject which has been passed with the SafetyStatusEvent event delegate is passed in myClickedPrefab argument
    // Also calls ChangeDictValue() function
    private void SetCurrentID(GameObject myClickedPrefab)
    {
        // Determine GameObject ID
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            // Debug.Log(equipment.Name);
            currentID = equipment.ID;
        } // Should I add else statement to any of these?

        ChangeDictValue(); // Seems a little messy to call these three functions from here
        AddSafeEquipCount();
        AreAllEquipSafe();
    }

    // Called after SetCurrentID finds the clicked on GameObject ID value
    // Clicked on GameObject ID value SHOULD correspond with specific key value in safetyStatus dictionary
    private void ChangeDictValue()
    {
        //Debug.Log(safetyStates[1]);
        //Debug.Log(safetyStates[2]);
        safetyStates[currentID] = true;
        //Debug.Log(safetyStates[1]);
        //Debug.Log(safetyStates[2]);
    }

    // Called at end of SetCurrentID which feels messy
    // Simply increases safeEquipCount value by one
    private void AddSafeEquipCount()
    {
        safeEquipCount += 1;
        Debug.Log(safeEquipCount);
    }

    // Called at end of SetCurrentID which feels messy
    // If value of safeEquipCount is equal to dictionary count all equipment objects have been sucesfully inspected
    private void AreAllEquipSafe()
    {
        if(safeEquipCount == safetyStates.Count)
        {
            isAllSafe = true;
            OnStatusChanged?.Invoke();
            // Debug.Log("We're all safe!");
        }
        else if(safeEquipCount < safetyStates.Count)
        {
            Debug.Log("Someone's still unsafe");
        }
    }

    #region Code Graveyard

    //public void SetCurrentPrefab(GameObject myClickedPrefab)
    //{
    //    currentPrefab = myClickedPrefab;
    //}

    //public GameObject GetCurrentPrefab()
    //{
    //    return currentPrefab;
    //}

    #endregion
}
