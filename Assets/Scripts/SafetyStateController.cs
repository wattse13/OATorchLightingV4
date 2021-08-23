using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to SafetyStateController GameObject
public class SafetyStateController : MonoBehaviour
{
    Dictionary<int, bool> safetyStates = new Dictionary<int, bool>();
    Dictionary<int, bool> ActiveStatesCorrect = new Dictionary<int, bool>();
    List<int> CurrentActiveEquipment = new List<int>();
    List<int> ActiveStatesPlayer = new List<int>();

    private int currentID;
    private int previousID;
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
        // This doesn't scale well
        // Could use a for loop, but would need to figure out some way to keep correct order
        safetyStates.Add(1, false); // Corresponds to Oxygen Cylinder
        safetyStates.Add(2, false); // Corresponds to Oxygen Regulator
        safetyStates.Add(3, false); // Corresponds to Acetylene Cylinder
        safetyStates.Add(4, false); // Corresponds to Acetylene Regulator
        safetyStates.Add(5, false); // Corresponds to Oxygen Hose
        safetyStates.Add(6, false); // Corresponds to Acetylene Hose
        safetyStates.Add(7, false); // Corresponds to OA torch
        safetyStates.Add(8, false); // Corresponds to Striker

        ActiveStatesCorrect.Add(1, false); // Corresponds to Oxygen Cylinder
        ActiveStatesCorrect.Add(2, false); // Corresponds to Oxygen Regulator
        ActiveStatesCorrect.Add(3, false); // Corresponds to Acetylene Cylinder
        ActiveStatesCorrect.Add(4, false); // Corresponds to Acetylene Regulator
        ActiveStatesCorrect.Add(5, false); // Corresponds to Oxygen Hose
        ActiveStatesCorrect.Add(6, false); // Corresponds to Acetylene Hose
        ActiveStatesCorrect.Add(7, false); // Corresponds to OA Torch
        ActiveStatesCorrect.Add(8, false); // Corresponds to Striker
    }

    private void OnEnable()
    {
        EquipmentController.OnSafetyValueChanged += SetSafetyID;
        EquipmentController.OnActiveValueChanged += SetActiveID;
        // OnClickDelegate.OnClicked += SetCurrentPrefab; // Not currently needed
    }

    private void OnDisable()
    {
        EquipmentController.OnSafetyValueChanged -= SetSafetyID;
        EquipmentController.OnActiveValueChanged -= SetActiveID;
        // OnClickDelegate.OnClicked -= SetCurrentPrefab; // Not currently needed
    }

    // This function finds the ID integer value of the passed in GameObject
    // The clicked on GameObject which has been passed with the SafetyStatusEvent event delegate is passed in myClickedPrefab argument
    // Also calls ChangeDictValue() function
    private void SetSafetyID(GameObject myClickedPrefab)
    {
        // Determine GameObject ID
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            // Debug.Log(equipment.Name);
            currentID = equipment.ID;
        } // Should I add else statement to any of these?

        ChangeSafeDictValue(); // Seems a little messy to call these three functions from here
        AddSafeEquipCount();
        AreAllEquipSafe();
    }

    // Called after SetCurrentID finds the clicked on GameObject ID value
    // Clicked on GameObject ID value SHOULD correspond with specific key value in safetyStatus dictionary
    private void ChangeSafeDictValue()
    {
        safetyStates[currentID] = true;
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

    // Essentially the same function as SetSafetyID, but used for GameObject Active status
    private void SetActiveID(GameObject myClickedPrefab)
    {
        if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            currentID = equipment.ID;
        }
        previousID = currentID - 1;

        ChangeActiveDictValue();
        CheckActiveOrder();
    }

    private void ChangeActiveDictValue()
    {
        if(ActiveStatesCorrect[currentID] == false)
        {
            ActiveStatesCorrect[currentID] = true;
            // AddToActiveList();
        }
        else if(ActiveStatesCorrect[currentID] == true)
        {
            ActiveStatesCorrect[currentID] = false;
            // RemoveFromActiveLists();
        }
        // Debug.Log(activeStates[currentID]);
    }

    // Doesn't recognize problem if equipment is activated in correct order, but later deactivated
    // Need more steps to effectively play test
    private void CheckActiveOrder()
    {
        if (isAllSafe == false)
        {
            Debug.Log("Inpsect first!");
        }
        if (previousID == 0) // This is creating a bug. 
        {
            return;
        }
        if(ActiveStatesCorrect[previousID] == false && isAllSafe == true)
        {
            WhatsActive();
            // CompareLists(ActiveStatesPlayer, ActiveStatesCorrectList);
        }
        if (ActiveStatesCorrect[previousID] == true && isAllSafe == false)
        {
            Debug.Log("Someone isn't safe");
            // UnsafeConsequences();
        }
        if(ActiveStatesCorrect[previousID] == true && isAllSafe == true)
        {
            AllSafeConsequences();
        }
        //foreach (var item in activeStates)
        //{
        //    // Debug.Log("Equipment: " + item.Key + " Status: " + item.Value + "\n");
        //}
    }

    private void WhatsActive()
    {
        foreach (var item in ActiveStatesCorrect)
        {
            if (item.Value == true)
            {
                CurrentActiveEquipment.Add(item.Key);
            }
        }
        AllSafeConsequences();
        //foreach(var item in CurrentActiveEquipment)
        //{
        //    Debug.Log(item);
        //}
    }

    // Determines result of incorrect activation order when all equipment is safe
    private void AllSafeConsequences()
    {
        if(!CurrentActiveEquipment.Contains(1) && !CurrentActiveEquipment.Contains(3))
        {
            Debug.Log("No Gas in System");
        }
        if(CurrentActiveEquipment.Contains(1) && CurrentActiveEquipment.Contains(2))
        {
            Debug.Log("Oxy Regulator Burnout");
        }
    }

    // Determines result of incorrect activation order when some or all equipment is unsafe
    private void UnsafeConsequences()
    {
        
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

    //private void AddToActiveList()
    //{
    //    ActiveStatesPlayer.Add(currentID);
    //    //ActiveStatesCorrectList.Add(currentID);

    //    //foreach(var item in ActiveStatesCorrect)
    //    //{
    //    //    if(item.Value == true)
    //    //    {
    //    //        ActiveStatesPlayer.Add(item.Key);
    //    //    }
    //    //}
    //}

    //private void RemoveFromActiveLists()
    //{
    //    ActiveStatesPlayer.Remove(currentID);
    //}

    //private bool CompareLists(List<int> a, List<int> b)
    //{
    //    for (var i = 0; i < a.Count; i++)
    //    {
    //        if (a[i] != b[i])
    //        {
    //            Debug.Log("Wrong!");
    //            return false;
    //        }
    //    }
    //    Debug.Log("yep");
    //    return true;
    //}

    #endregion
}
