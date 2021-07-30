﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Attatched to InspectMenuController GameObject
// Also used to control UseMenu
public class InspectMenuController : MonoBehaviour
{
    private GameObject currentPrefab;
    public GameObject blurLayer;
    private GameObject inspectMenu;
    private GameObject useMenu;

    public GameObject replaceButton;

    private TMP_Text myInspectTitle;
    private TMP_Text myInspectText;
    private TMP_Text myUseTitle;
    private TMP_Text myUseText;

    // private bool isPrefabActive;
    private bool isPrefabSafe;
    private string currentPrefabName;
    private string currentPrefabDescr;
    private string currentPrefabDescrUnsafe;
    private string currentPrefabDescrSafe;
    private string currentPrefabUseDescr;
    private string currentPrefabUseUnsafeDescr;
    private string currentPrefabUseSafeDescr;

    private bool isAllEquipSafe = false;

    // Delegates //

    // Will be used to send a message when a GameObject's active status has been changed
    public delegate void ActiveStatusEvent(GameObject e);
    public static event ActiveStatusEvent OnActivate;

    // Nothing currently subscribed to this delegate event
    // Will be used to send a message when inspect menu has been activated
    // Not sure this is necessary
    public delegate void InspectMenuEvent(GameObject e);
    public static event InspectMenuEvent OnInspectMenuActivate;

    private void Awake()
    {
        blurLayer.SetActive(false);
    }

    private void OnEnable()
    {
        // GameEvents.OnMessageSent += SetCurrentPrefab;
        GameEvents.OnMessageSent += SetCurrentPrefabValues;
        SafetyStateController.OnStatusChanged += AllSafe;
        EquipmentController.OnValueChanged += UpdatePrefabSafetyStatus;
    }
    private void OnDisable()
    {
        // GameEvents.OnMessageSent -= SetCurrentPrefab;
        GameEvents.OnMessageSent -= SetCurrentPrefabValues;
        SafetyStateController.OnStatusChanged -= AllSafe;
        EquipmentController.OnValueChanged -= UpdatePrefabSafetyStatus;
    }

    private void Start()
    {
        inspectMenu = GameObject.Find("InspectMenu");
        useMenu = GameObject.Find("UseMenu");

        myInspectTitle = GameObject.Find("EquipInspectName").GetComponent<TMP_Text>();
        myInspectText = GameObject.Find("InspectDescription").GetComponent<TMP_Text>();
        myUseTitle = GameObject.Find("EquipUseName").GetComponent<TMP_Text>();
        myUseText = GameObject.Find("UseDescription").GetComponent<TMP_Text>();
    }

    public void SetCurrentPrefabValues(GameObject myClickedPrefab)
    {
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            currentPrefabName = equipment.Name;
            currentPrefabDescr = equipment.DescriptionCurrent;
            currentPrefabDescrSafe = equipment.DescriptionSafe;
            currentPrefabDescrUnsafe = equipment.DescriptionUnsafe;
            currentPrefabUseSafeDescr = equipment.UseSafeDescr;
            currentPrefabUseUnsafeDescr = equipment.UseUnsafeDescr;
            isPrefabSafe = equipment.IsSafe;
        }
    }

    // Called after OnValueChanged event is triggered
    public void UpdatePrefabSafetyStatus(GameObject myClickedPrefab)
    {
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            isPrefabSafe = equipment.IsSafe;
            SetInspectText();
            SetUseText();
            // Debug.Log("Updating");
        }
    }

    // Called after OnStatusChanged delegate event is invoked
    public void AllSafe()
    {
        isAllEquipSafe = true;
        Debug.Log("We're all safe!");
        // Maybe show a pop up congratulations?
    }

    // Called at end of UpdatePrefabSafetyStatus() function
    // Called by ClickMenu Inspect Button click
    // Changes InspectMenu text based on GameObject SafetyStatus
    // Also disables Replace button based on GameObject SafetyStatus
    public void SetInspectText()
    {
        myInspectTitle.text = currentPrefabName;

        if(isPrefabSafe == false)
        {
            myInspectText.text = currentPrefabDescrUnsafe;
            // Tried doing this in a seperate function, but couldn't re-enable button after first disable
            replaceButton.SetActive(true);
            // Debug.Log("I'm still unsafe");
        }
        else if(isPrefabSafe == true)
        {
            myInspectText.text = currentPrefabDescrSafe;
            replaceButton.SetActive(false);
            // Debug.Log("It's safe now");
        }
    }

    // Called at end of UpdatePrefabsSafetyStatus() function
    // Called by ClickMenu Use Button Click
    public void SetUseText()
    {
        // Should work similarly to SetInpsectText()
        myUseTitle.text = currentPrefabName; 
        // Check if current prefab is safe and check value of isAllEquipSafe
        if(isPrefabSafe == false && isAllEquipSafe == false)
        {
            myUseText.text = currentPrefabUseUnsafeDescr + " Make sure to inspect ALL equipment before using.";
        }
        else if(isPrefabSafe == true && isAllEquipSafe == false)
        {
            // Minor bug: Could be confusing when only one piece of equipment remaains to be inspected
            myUseText.text = currentPrefabUseSafeDescr + " However, not all equipment has been inspected.";
        }
        else if(isPrefabSafe == true && isAllEquipSafe == true)
        {
            myUseText.text = currentPrefabUseSafeDescr;
        }
        // Check value of isAllEquipSafe
        // Set text based on above two conditions
    }

    public void AddBackgroundBlur()
    {
        blurLayer.SetActive(true);
    }

    public void RemoveBlur()
    {
        blurLayer.SetActive(false);
    }

    //public void ValueTest()
    //{
    //    Debug.Log(isPrefabSafe);
    //    Debug.Log(currentPrefabName);
    //    // Debug.Log(currentPrefabDescrUnsafe);
    //}

    #region Code Grave Yard
    //// This method is called once a the OnMessageSent delegate event is triggered
    //public void SetCurrentPrefab(GameObject myClickedPrefab)
    //{
    //    currentPrefab = myClickedPrefab;
    //}

    //// When this function is called by SetCurrentPrefab it should retrun the passed in GameObject parameter as a value
    //public GameObject GetCurrentPrefab()
    //{
    //    // Debug.Log("Made it to GetCurrentPrefab");
    //    return currentPrefab;
    //}

    //// Assigns currentPrefab variable to value of the GetCurrentPrefab() return value
    //// Calls all functions related to centering and emphasizing inspected GameObject and passes currentPrefab in as argument
    //public void CallMenuFunctions()
    //{
    //    currentPrefab = GetCurrentPrefab();
    //    // CenterPrefab(currentPrefab);
    //    // FocusLayer(currentPrefab);
    //    // DisableCollider(currentPrefab);
    //    // AddBackgroundBlur();
    //}

    //public void ExitMenuFunctions()
    //{
    //    currentPrefab = GetCurrentPrefab();
    //    // ReturnPrefabPosition(currentPrefab);
    //    // ReturnPrefabSortLayer(currentPrefab);
    //    // ReturnPrefabCollider(currentPrefab);
    //    // RemoveBlur();
    //}
    #endregion
}
