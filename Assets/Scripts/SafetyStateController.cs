using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to SafetyStateController GameObject
public class SafetyStateController : MonoBehaviour
{
    Dictionary<int, bool> safetyStates = new Dictionary<int, bool>();
    Dictionary<int, bool> ActiveStatesCorrect = new Dictionary<int, bool>();
    HashSet<int> CurrentActiveEquipment = new HashSet<int>();
    List<int> ActiveStatesPlayer = new List<int>();

    List<int> OxyRegBurnout = new List<int>();
    List<int> OxyRegBurnoutTrue = new List<int>();
    List<int> AcetylRegBurnout = new List<int>();
    List<int> AcetylRegBurnoutTrue = new List<int>();

    private int currentID;
    private int safeEquipCount = 0;

    //private bool isAllSafe = false;
    //private bool isAcetylBurnout = false;
    //private bool isOxyBurnout = false;
    private bool isAcetylLeak = false;
    private bool isOxyLeak = false;
    private bool isAcetylPressure = false;
    private bool isOxyPressure = false;

    private int oxyCylinder = 1;
    private int oxyReg = 2;
    private int acetylCylinder = 3;
    private int acetylReg = 4;
    private int oxyHose = 5;
    private int acetylHose = 6;
    private int torch = 7;
    private int lighter = 8;

    // Should use events rather than this fragile id system
    private int oxyLeakEvent = 1;
    private int acetylLeakEvent = 3;
    // private int oxyBurnout = 2;
    // private int acetylBurnout = 4;
    private int oxyHoseLeak = 5;
    private int acetylHoseLeak = 6;
    private int handExplosion = 7;
    private int smallExplosion = 8;
    private int bigExplosion = 9;
    private int noAcetyl = 10;

    // InspectMenuController is subscribed to this delegate event
    // WinState is subscribed to this event
    // Will be used to send a message when all GameObject's safety status have been changed
    public delegate void SafetyStatusEvent();
    public static event SafetyStatusEvent OnStatusChanged;

    // WinState is subscribed to this delegate event
    // Used to tell subscribers all important objects are active
    public delegate void ActiveStatusEvent();
    public static event ActiveStatusEvent OnActiveChanged;

    // GameEvents is subscribed to this delegate event
    // Passes integer variable which corresponds to integer variable in GameEvents
    // Determines what kind of accident hass occured
    // The matching integer variable system is very fragile
    public delegate void ConsequenceEvent(int whichAccident);
    public static event ConsequenceEvent OnConsequence;

    private void Awake()
    {
        // Integer key must correspond to GameObject ID
        // Very Important Key integer matches GameObject ID integer
        // This doesn't scale well
        // Could use a for loop, but would need to figure out some way to keep correct order
        safetyStates.Add(oxyCylinder, false); // Corresponds to Oxygen Cylinder
        safetyStates.Add(oxyReg, false); // Corresponds to Oxygen Regulator
        safetyStates.Add(acetylCylinder, false); // Corresponds to Acetylene Cylinder
        safetyStates.Add(acetylReg, false); // Corresponds to Acetylene Regulator
        safetyStates.Add(oxyHose, false); // Corresponds to Oxygen Hose
        safetyStates.Add(acetylHose, false); // Corresponds to Acetylene Hose
        safetyStates.Add(torch, false); // Corresponds to OA torch
        safetyStates.Add(lighter, false); // Corresponds to Striker

        ActiveStatesCorrect.Add(oxyCylinder, false); // Corresponds to Oxygen Cylinder
        ActiveStatesCorrect.Add(oxyReg, false); // Corresponds to Oxygen Regulator
        ActiveStatesCorrect.Add(acetylCylinder, false); // Corresponds to Acetylene Cylinder
        ActiveStatesCorrect.Add(acetylReg, false); // Corresponds to Acetylene Regulator
        ActiveStatesCorrect.Add(oxyHose, false); // Corresponds to Oxygen Hose
        ActiveStatesCorrect.Add(acetylHose, false); // Corresponds to Acetylene Hose
        ActiveStatesCorrect.Add(torch, false); // Corresponds to OA Torch
        ActiveStatesCorrect.Add(lighter, false); // Corresponds to Striker

        OxyRegBurnoutTrue.Add(2);
        OxyRegBurnoutTrue.Add(1);

        AcetylRegBurnoutTrue.Add(4);
        AcetylRegBurnoutTrue.Add(3);
    }

    private void OnEnable()
    {
        EquipmentController.OnSafetyValueChanged += SetSafetyID;
        EquipmentController.OnActiveValueChanged += SetActiveID;
        EquipmentController.OnActiveValueChanged += ConsequenceCoordinator;
    }

    private void OnDisable()
    {
        EquipmentController.OnSafetyValueChanged -= SetSafetyID;
        EquipmentController.OnActiveValueChanged -= SetActiveID;
        EquipmentController.OnActiveValueChanged -= ConsequenceCoordinator;
    }

    // This function finds the ID integer value of the passed in GameObject
    // The clicked on GameObject which has been passed with the SafetyStatusEvent event delegate is passed in myClickedPrefab argument
    // Also calls ChangeDictValue() function
    private void SetSafetyID(GameObject myClickedPrefab)
    {
        // Determine GameObject ID
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            currentID = equipment.ID;
        } 

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
    }

    // Called at end of SetCurrentID which feels messy
    // If value of safeEquipCount is equal to dictionary count all equipment objects have been sucesfully inspected
    private void AreAllEquipSafe()
    {
        if(safeEquipCount == safetyStates.Count)
        {
            OnStatusChanged?.Invoke();
        }
        else if(safeEquipCount < safetyStates.Count)
        {
            Debug.Log("Someone's still unsafe");
        }
    }

    // Called at end of WhatsActive
    // If hash set contains necessary integers, it invokes delegate event
    // Delegate event tells win state to change boolean value isAllActive to true
    private void AreEquipActive()
    {
        if (CurrentActiveEquipment.Contains(oxyCylinder) && CurrentActiveEquipment.Contains(oxyReg) && CurrentActiveEquipment.Contains(acetylCylinder) &&
            CurrentActiveEquipment.Contains(acetylReg) && CurrentActiveEquipment.Contains(torch) && CurrentActiveEquipment.Contains(lighter))
        {
            OnActiveChanged?.Invoke();
        }
        if ((CurrentActiveEquipment.Contains(oxyCylinder) && CurrentActiveEquipment.Contains(oxyReg) && CurrentActiveEquipment.Contains(acetylCylinder) &&
            CurrentActiveEquipment.Contains(acetylReg) && CurrentActiveEquipment.Contains(torch) && CurrentActiveEquipment.Contains(lighter)) && 
            (CurrentActiveEquipment.Contains(oxyHose) || CurrentActiveEquipment.Contains(acetylHose)))
        {
            OnActiveChanged?.Invoke();
        }
    }

    // Essentially the same function as SetSafetyID, but used for GameObject Active status
    private void SetActiveID(GameObject myClickedPrefab)
    {
        if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            currentID = equipment.ID;
        }
        ChangeActiveDictValue();
    }

    // Calls different methods based on GameObject's ID value
    private void ConsequenceCoordinator(GameObject myClickedPrefab)
    {
        if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            if (equipment.ID == oxyCylinder || equipment.ID == oxyReg)
            {
                OxyRegBurnoutTest(equipment.ID);
            }
            if (equipment.ID == acetylCylinder || equipment.ID == acetylReg)
            {
                AcetylRegBurnoutTest(equipment.ID);
            }
            if (CurrentActiveEquipment.Contains(torch) && (CurrentActiveEquipment.Contains(oxyReg) || CurrentActiveEquipment.Contains(acetylReg)))
            {
                LeakTest(torch);
            }
            if (CurrentActiveEquipment.Contains(lighter))
            {
                ExplosionTest();
            }
        }
    }

    // Changes the dictionary value in ActiveStatesCorrect
    // Uses currentID as key which is a rather fragile system probably definately
    private void ChangeActiveDictValue()
    {
        if(ActiveStatesCorrect[currentID] == false)
        {
            ActiveStatesCorrect[currentID] = true;
            UpdateActivePlayerList(currentID);
            WhatsActive();
            SetPressureVariables(currentID);
        }
        else if(ActiveStatesCorrect[currentID] == true)
        {
            ActiveStatesCorrect[currentID] = false;
            UpdateActivePlayerList(currentID);
            WhatsActive();
            SetPressureVariables(currentID);
        }
    }

    // Updates ActiveStatesPlayer list according to what keys in ActiveStatesCorrect have a value of true
    private void UpdateActivePlayerList(int id)
    {
        if (!ActiveStatesPlayer.Contains(id))
        {
            ActiveStatesPlayer.Add(id);
            foreach (var item in ActiveStatesPlayer)
            {
                Debug.Log(item);
            }
        }
        else if (ActiveStatesPlayer.Contains(id))
        {
            ActiveStatesPlayer.Remove(id);
        }
    }

    // These two functions do essentially the same thing
    // If list contains int, int is removed. If list does not contain int, int is added
    private void OxyRegBurnoutTest(int id)
    {
        if (!OxyRegBurnout.Contains(id))
        {
            OxyRegBurnout.Add(id);
        }
        else if (OxyRegBurnout.Contains(id))
        {
            OxyRegBurnout.Remove(id);
        }
        CompareLists(OxyRegBurnout, OxyRegBurnoutTrue, oxyReg);
    }

    private void AcetylRegBurnoutTest(int id)
    {
        if (!AcetylRegBurnout.Contains(id))
        {
            AcetylRegBurnout.Add(id);
        }
        else if (OxyRegBurnout.Contains(id))
        {
            AcetylRegBurnout.Remove(id);
        }
        CompareLists(AcetylRegBurnout, AcetylRegBurnoutTrue, acetylReg);
    }

    // Compares lists created by oxy/acetylRegBurnoutTest to oxy/acetylRegBurnoutTrue lists
    // Burnout event occurs if the lists are equal
    private void CompareLists(List<int> a, List<int> b, int reg)
    {
        if (a.Count != b.Count)
        {
            Debug.Log("Lists Unequal");
            return;
        }
        if (a.Count == b.Count)
        {
            for (var i =0; i < a.Count; i ++)
            {
                if (a[i] != b[i] && safetyStates[reg] == true)
                {
                    return;
                }
                if (a[i] != b[i] && safetyStates[reg] == false)
                {
                    OnConsequence?.Invoke(reg);
                    return;
                }
                if (a[i] == b[i])
                {
                    OnConsequence?.Invoke(reg);
                    return;
                }
            }
        }
    }

    // No way to set pressure variables back to false?
    private void SetPressureVariables(int reg)
    {
        if (reg == oxyReg)
        {
            if (ActiveStatesCorrect[oxyReg] == true && ActiveStatesCorrect[oxyCylinder] == true)
            {
                isOxyPressure = true;
                // Debug.Log("oxy " + isOxyPressure);
            }
            else
            {
                isOxyPressure = false;
                // Debug.Log("oxy " + isOxyPressure);
            }
            
        }
        if (reg == acetylReg)
        {
            if (ActiveStatesCorrect[acetylReg] == true && ActiveStatesCorrect[acetylCylinder] == true)
            {
                isAcetylPressure = true;
                // Debug.Log("acetyl " + isAcetylPressure);
            }
            else
            {
                isAcetylPressure = false;
                // Debug.Log("acetyl " + isAcetylPressure);
            }
        }
    }

    // Checks position of torch in ActiveStatesPlayer list
    private void LeakTest(int id)
    {
        // Would later like to turn this if check into a bool check (isOxyPressure)
        if (isOxyPressure == true)
        {
            // Debug.Log("Made it past first leak check");
            if ((ActiveStatesPlayer.IndexOf(id) < ActiveStatesPlayer.IndexOf(oxyReg)) || safetyStates[torch] == false)
            {
                isOxyLeak = true;
                OnConsequence?.Invoke(oxyLeakEvent);
                // Debug.Log("Oxygen Leak from Torch?");
            }
            else
            {
                isOxyLeak = false;
                // OnLeaked?.Invoke(oxyLeak, isOxyLeak);
            }
        }
        if (isAcetylPressure == true)
        {
            if ((ActiveStatesPlayer.IndexOf(id) < ActiveStatesPlayer.IndexOf(acetylReg)) || safetyStates[torch] == false)
            {
                isAcetylLeak = true;
                OnConsequence?.Invoke(acetylLeakEvent);
                // Debug.Log("Acetylene Leak from Torch?");
            }
            else
            {
                isAcetylLeak = false;
                // OnLeaked?.Invoke(acetylLeak, isAcetylLeak);
            }
        }
        if (isOxyPressure == true && safetyStates[oxyHose] == false)
        {
            OnConsequence?.Invoke(oxyHoseLeak);
        }
        if (isAcetylPressure == true && safetyStates[acetylHose] == false)
        {
            OnConsequence?.Invoke(acetylHoseLeak);
        }
    }

    private void ExplosionTest()
    {
        if (isAcetylLeak != true)
        {
            Debug.Log("No Explosion");
            // return;
        }
        if (isOxyLeak == true)
        {
            OnConsequence?.Invoke(noAcetyl);
        }
        if (isAcetylLeak == true)
        {
            OnConsequence?.Invoke(smallExplosion);
        }
        if (isAcetylLeak == true && isOxyLeak == true)
        {
            OnConsequence?.Invoke(bigExplosion);
        }
        if ((isAcetylPressure == true && isOxyPressure == true && safetyStates[torch] == true && safetyStates[lighter] == false) ||
            (isAcetylPressure == true && safetyStates[torch] == true && safetyStates[lighter] == false))
        {
            OnConsequence?.Invoke(handExplosion);
        }
    }

    // Iterates through ActiveStatesCorrect dictionary and adds keys with true value to new hash set
    private void WhatsActive()
    {
        foreach (var item in ActiveStatesCorrect)
        {
            if (item.Value == true)
            {
                CurrentActiveEquipment.Add(item.Key);
                AreEquipActive();
            }
        }
        foreach (var item in ActiveStatesCorrect)
        {
            if (item.Value == false)
            {
                CurrentActiveEquipment.Remove(item.Key);
                AreEquipActive();
            }
        }
    }
}
