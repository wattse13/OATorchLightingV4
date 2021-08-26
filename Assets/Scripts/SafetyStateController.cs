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
    // List<int> ActiveStatesCorrectShort = new List<int>();

    List<int> OxyRegBurnout = new List<int>();
    List<int> OxyRegBurnoutTrue = new List<int>();
    List<int> AcetylRegBurnout = new List<int>();
    List<int> AcetylRegBurnoutTrue = new List<int>();

    private int currentID;
    // private int previousID;
    // private int nextID;
    private int safeEquipCount = 0;

    private bool isAllSafe = false;
    private bool isAcetylBurnout = false;
    private bool isOxyBurnout = false;
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

    // Were going to be sent with to timer
    // private int oxyLeak = 1;
    // private int acetylLeak = 2;


    // public GameObject currentPrefab; // Not currently needed

    // InspectMenuController is subscribed to this delegate event
    // Will be used to send a message when a GameObject's safety status has been changed
    public delegate void SafetyStatusEvent();
    public static event SafetyStatusEvent OnStatusChanged;
    // OnStatusChanged?.Invoke(this.gameObject);

    // Timer is subscribed to this delegate event
    // public delegate void LeakEvent(int id, bool leak);
    // public static event LeakEvent OnLeaked;
    // public static event LeakEvent OnStoppedLeak;

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
        // OnClickDelegate.OnClicked += SetCurrentPrefab; // Not currently needed
    }

    private void OnDisable()
    {
        EquipmentController.OnSafetyValueChanged -= SetSafetyID;
        EquipmentController.OnActiveValueChanged -= SetActiveID;
        EquipmentController.OnActiveValueChanged -= ConsequenceCoordinator;
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
        // previousID = currentID - 1;
        // nextID = currentID + 1;

        ChangeActiveDictValue();
        // CheckActiveOrder();
    }

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

    private void ChangeActiveDictValue()
    {
        if(ActiveStatesCorrect[currentID] == false)
        {
            ActiveStatesCorrect[currentID] = true;
            UpdateActivePlayerList(currentID);
            WhatsActive();
            // CreateCompareLists();
            // CompareLists(ActiveStatesPlayer, ActiveStatesCorrectShort);
        }
        else if(ActiveStatesCorrect[currentID] == true)
        {
            ActiveStatesCorrect[currentID] = false;
            UpdateActivePlayerList(currentID);
            WhatsActive();
            // CreateCompareLists();
            // CompareLists(ActiveStatesPlayer, ActiveStatesCorrectShort);
        }
        // Debug.Log(activeStates[currentID]);
    }

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
        CompareLists(OxyRegBurnout, OxyRegBurnoutTrue, isOxyBurnout, isOxyPressure, id);
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
        CompareLists(AcetylRegBurnout, AcetylRegBurnoutTrue, isAcetylBurnout, isAcetylPressure, id);
    }

    // This disaster takes the lists provided by AcetylReg- or OxyReg- and compares them
    // Depending on the list comparison, and a safetyState dictionary look up, the regulator either burns out or doesn't
    private void CompareLists(List<int> a, List<int> b, bool oops, bool correct, int id)
    {
        if (a.Count != b.Count)
        {
            // Debug.Log("Lists Unequal");
            oops = false; // Doesn't actually change bool?
            correct = true; // Doesn't actually change bool?
            return;
        }
        if (id == oxyReg)
        {
            for (var i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i])
                {
                    if (safetyStates[oxyReg] == true)
                    {
                        Debug.Log("No Oxy Burnout Yet!");
                        oops = false;
                        correct = true;
                        isOxyPressure = true;
                        //Debug.Log(oops);
                        //Debug.Log(correct);
                        //Debug.Log(isOxyPressure);
                        return;
                    }
                    if (safetyStates[oxyReg] == false)
                    {
                        Debug.Log("Oxy Reg Burnout!");
                        oops = true;
                        correct = false;
                        return;
                    }
                    Debug.Log("Burnout Oxy Reg!");
                    oops = true;
                    correct = false;
                    return;
                }
            }
        }
        if (id == acetylReg)
        {
            for (var i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i])
                {
                    if (safetyStates[acetylReg] == true)
                    {
                        Debug.Log("No Acetyl Burnout Yet!");
                        oops = false;
                        correct = true;
                        isAcetylPressure = true;
                        return;
                    }
                    if (safetyStates[acetylReg] == false)
                    {
                        Debug.Log("Acetyl Reg Burnout!");
                        oops = true;
                        correct = false;
                        return;
                    }
                    Debug.Log("Burnout Acetyl Reg!");
                    oops = true;
                    correct = false;
                    return;
                }
            }
        }
        oops = true;
        correct = false;
        Debug.Log(id + " Burnout!");
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
                // OnLeaked?.Invoke(oxyLeak, isOxyLeak);
                Debug.Log("Oxygen Leak from Torch?");
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
                // OnLeaked?.Invoke(acetylLeak, isAcetylLeak);
                Debug.Log("Acetylene Leak from Torch?");
            }
            else
            {
                isAcetylLeak = false;
                // OnLeaked?.Invoke(acetylLeak, isAcetylLeak);
            }
        }
        if (isOxyPressure == true && safetyStates[oxyHose] == false)
        {
            Debug.Log("Oxy Leak from Hose");
        }
        if (isAcetylPressure == true && safetyStates[acetylHose] == false)
        {
            Debug.Log("Acetyl Leak from Hose");
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
            Debug.Log("Torch Won't Light");
        }
        if (isAcetylLeak == true)
        {
            Debug.Log("Boom!");
        }
        if (isAcetylLeak == true && isOxyLeak == true)
        {
            Debug.Log("BIG BOOM!");
        }
        if ((isAcetylPressure == true && isOxyPressure == true && safetyStates[torch] == true && safetyStates[lighter] == false) ||
            (isAcetylPressure == true && safetyStates[torch] == true && safetyStates[lighter] == false))
        {
            Debug.Log("Explosion in Hand");
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
            }
        }
        foreach (var item in ActiveStatesCorrect)
        {
            if (item.Value == false)
            {
                CurrentActiveEquipment.Remove(item.Key);
            }
        }
        //if (isAllSafe == true)
        //{
        //    AllSafeConsequences();
        //}
        //else { AllSafeConsequences(); }

        //foreach (var item in CurrentActiveEquipment)
        //{
        //    Debug.Log(item + "WhatsActive List");
        //}
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


    //// Doesn't recognize problem if equipment is activated in correct order, but later deactivated
    //// Need more steps to effectively play test
    //private void CheckActiveOrder()
    //{
    //    //if (isAllSafe == false)
    //    //{
    //    //    Debug.Log("Inpsect first!");
    //    //}
    //    if (previousID == 0)
    //    {
    //        if (ActiveStatesCorrect[nextID] == true)
    //        {
    //            WhatsActive();
    //            Debug.Log("Oxy Regulator Burnout");
    //        }
    //        return;
    //    }
    //    if (currentID == 3)
    //    {
    //        if (ActiveStatesCorrect[nextID] == true)
    //        {
    //            WhatsActive();
    //            Debug.Log("Acetylene Regulator Burnout");
    //        }
    //    }
    //    if (ActiveStatesCorrect[previousID] == false && isAllSafe == true)
    //    {
    //        WhatsActive();
    //        // CompareLists(ActiveStatesPlayer, ActiveStatesCorrectList);
    //    }
    //    if (ActiveStatesCorrect[previousID] == true && isAllSafe == false)
    //    {
    //        Debug.Log("Someone isn't safe");
    //        // UnsafeConsequences();
    //    }
    //    if (ActiveStatesCorrect[previousID] == true && isAllSafe == true)
    //    {
    //        return;
    //        // I think this means player was succesful?
    //        // AllSafeConsequences();
    //    }
    //}

    //if (!CurrentActiveEquipment.Contains(1) && !CurrentActiveEquipment.Contains(3))
    //{
    //    Debug.Log("No Gas in System");
    //    return; // Things only get dangerous once gas cylinders are opened
    //}
    //if (!CurrentActiveEquipment.Contains(1)) { Debug.Log("No Oxy in System"); }
    //if (!CurrentActiveEquipment.Contains(2)) { Debug.Log("No Acetylene in System"); }
    //if (!CurrentActiveEquipment.Contains(1) && !CurrentActiveEquipment.Contains(2) &&
    //    CurrentActiveEquipment.Contains(3) && CurrentActiveEquipment.Contains(4))
    //{
    //    Debug.Log("Acetylene turned on before Oxygen");
    //    // return;
    //}
    //if (CurrentActiveEquipment.Contains(1) && CurrentActiveEquipment.Contains(2) && CurrentActiveEquipment.Contains(3) &&
    //    CurrentActiveEquipment.Contains(4) && CurrentActiveEquipment.Contains(7) && CurrentActiveEquipment.Contains(8))
    //{
    //    Debug.Log("Explosion!");
    //    return;
    //}
    //if (!CurrentActiveEquipment.Contains(1) && CurrentActiveEquipment.Contains(2) && CurrentActiveEquipment.Contains(3) &&
    //   CurrentActiveEquipment.Contains(4) && CurrentActiveEquipment.Contains(7) && CurrentActiveEquipment.Contains(8))
    //{
    //    Debug.Log("Torch Lights. No Oxygen at Torch. Oxy Regulator Burnout Danger");
    //    return;
    //}
    //if (CurrentActiveEquipment.Contains(1) && !CurrentActiveEquipment.Contains(2) && CurrentActiveEquipment.Contains(3) &&
    //   CurrentActiveEquipment.Contains(4) && CurrentActiveEquipment.Contains(7) && CurrentActiveEquipment.Contains(8))
    //{
    //    Debug.Log("Torch Lights. No Oxygen at Torch.");
    //    return;
    //}
    //if (CurrentActiveEquipment.Contains(1) && CurrentActiveEquipment.Contains(2) && !CurrentActiveEquipment.Contains(3) &&
    //   CurrentActiveEquipment.Contains(4) && CurrentActiveEquipment.Contains(7) && CurrentActiveEquipment.Contains(8))
    //{
    //    Debug.Log("Torch won't light. Oxygen Leak. Acetylene Regulator Burnout Danger");
    //    return;
    //}
    //if (CurrentActiveEquipment.Contains(1) && CurrentActiveEquipment.Contains(2) && CurrentActiveEquipment.Contains(3) &&
    //   !CurrentActiveEquipment.Contains(4) && CurrentActiveEquipment.Contains(7) && CurrentActiveEquipment.Contains(8))
    //{
    //    Debug.Log("Torch won't light. Oxygen Leak");
    //    return;
    //}
    //if (CurrentActiveEquipment.Contains(1) && CurrentActiveEquipment.Contains(2) && CurrentActiveEquipment.Contains(3) &&
    //   CurrentActiveEquipment.Contains(4) && CurrentActiveEquipment.Contains(7) && !CurrentActiveEquipment.Contains(8))
    //{
    //    Debug.Log("Oxygen and Acetylene Leaking from Torch");
    //    return;
    //}
    //if (CurrentActiveEquipment.Contains(1) && CurrentActiveEquipment.Contains(2) && CurrentActiveEquipment.Contains(3) &&
    //   CurrentActiveEquipment.Contains(4) && !CurrentActiveEquipment.Contains(7) && !CurrentActiveEquipment.Contains(8))
    //{
    //    Debug.Log("Nothing Happend?");
    //    return;
    //}
    //else
    //{
    //    return;
    //}

    //// Creates two lists from ActiveStatesCorrect Dictionary
    //// List a contains all keys with a value of true
    //// List b contains integers starting with one and ending at the length of ActiveStatesPlayer list
    //private void CreateCompareLists()
    //{
    //    foreach (var item in ActiveStatesCorrect)
    //    {
    //        // Checks if value already exists within ActiveStatesPlayer before adding
    //        if (item.Value == true && !ActiveStatesPlayer.Contains(item.Key))
    //        {
    //            ActiveStatesPlayer.Add(item.Key);
    //        }
    //    }
    //    foreach (var item in ActiveStatesCorrect)
    //    {
    //        if (item.Key <= ActiveStatesPlayer.Count)
    //        {
    //            ActiveStatesCorrectShort.Add(item.Key);
    //        }
    //    }
    //}

    //private bool CompareLists(List<int> a, List<int> b)
    //{
    //    if (a.Count != b.Count)
    //    {
    //        Debug.Log("Lists not equal length");
    //        // a.Clear();
    //        b.Clear();
    //        return false;
    //    }
    //    for (var i = 0; i < a.Count; i++)
    //    {
    //        if (a[i] != b[i])
    //        {
    //            // a.Clear();
    //            b.Clear();
    //            Debug.Log("Lists not equal");
    //            WhatsActive();
    //            return false;
    //        }
    //    }
    //    Debug.Log("Made it through comparison?");
    //    foreach (var item in a)
    //    {
    //        Debug.Log(item + "list a");
    //    }
    //    foreach (var item in b)
    //    {
    //        Debug.Log(item + "list b");
    //    }
    //    // a.Clear();
    //    b.Clear();
    //    return true;
    //}

    //// Determines result of incorrect activation order when all equipment is safe
    //private void AllSafeConsequences()
    //{

    //}

    //// Determines result of incorrect activation order when some or all equipment is unsafe
    //private void UnsafeConsequences()
    //{
    //    Debug.Log("Something activated while there is still unsafe equipment");
    //}

    #endregion
}
