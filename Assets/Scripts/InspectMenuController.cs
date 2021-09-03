using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Attatched to InspectMenuController GameObject
// Also used to control UseMenu
public class InspectMenuController : MonoBehaviour
{
    [SerializeField] private GameObject blurLayer;

    public GameObject replaceButton;

    private TMP_Text myInspectTitle;
    private TMP_Text myInspectText;
    private TMP_Text myUseTitle;
    private TMP_Text myUseText;
    private TMP_Text myUseButtonText;

    private bool isPrefabSafe;
    private bool isPrefabActive;
    private string currentPrefabName;
    private string currentPrefabDescrUnsafe;
    private string currentPrefabDescrSafe;
    private string currentPrefabUseUnsafeDescr;
    private string currentPrefabUseSafeDescr;

    private bool isAllEquipSafe = false;

    // Delegates //

    // EquipmentController is subscribed to this delegate event 
    // Reenables colliders once Inspect/Use menu is deactivated
    public delegate void InspectMenuEvent();
    public static event InspectMenuEvent OnInspectMenuActivate;
    public static event InspectMenuEvent OnInspectMenuDeactivate; // Is this legit?

    // EquipmentController is subscribed to this delegate event
    // Updates equipment data through EquipmentController class
    public delegate void UseMenuEvent();
    public static event UseMenuEvent OnUseMenuActivate;

    private void Awake()
    {
        blurLayer.SetActive(false);
    }

    private void OnEnable()
    {
        OnClickDelegate.OnClicked += SetCurrentPrefabValues;
        SafetyStateController.OnStatusChanged += AllSafe;
        EquipmentController.OnSafetyValueChanged += UpdatePrefabSafetyStatus;
        EquipmentController.OnActiveValueChanged += UpdatePrefabActiveStatus;
    }
    private void OnDisable()
    {
        OnClickDelegate.OnClicked -= SetCurrentPrefabValues;
        SafetyStateController.OnStatusChanged -= AllSafe;
        EquipmentController.OnSafetyValueChanged -= UpdatePrefabSafetyStatus;
        EquipmentController.OnActiveValueChanged -= UpdatePrefabActiveStatus;
    }

    private void Start()
    {
        myInspectTitle = GameObject.Find("EquipInspectName").GetComponent<TMP_Text>();
        myInspectText = GameObject.Find("InspectDescription").GetComponent<TMP_Text>();
        myUseTitle = GameObject.Find("EquipUseName").GetComponent<TMP_Text>();
        myUseText = GameObject.Find("UseDescription").GetComponent<TMP_Text>();
        myUseButtonText = GameObject.Find("UseButton").GetComponentInChildren<TMP_Text>();
    }

    // Sets variables to hold data about clicked on object
    public void SetCurrentPrefabValues(GameObject myClickedPrefab)
    {
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            currentPrefabName = equipment.Name;
            currentPrefabDescrSafe = equipment.DescriptionSafe;
            currentPrefabDescrUnsafe = equipment.DescriptionUnsafe;
            currentPrefabUseSafeDescr = equipment.UseSafeDescr;
            currentPrefabUseUnsafeDescr = equipment.UseUnsafeDescr;
            isPrefabSafe = equipment.IsSafe;
            isPrefabActive = equipment.IsActive;
        }
        ChangeUseButton(); // Bit hacky, but makes sure Use Button updates before player clicks on new object
    }

    // Called after OnSafetyValueChanged event is triggered
    public void UpdatePrefabSafetyStatus(GameObject myClickedPrefab)
    {
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            isPrefabSafe = equipment.IsSafe;
            SetInspectText();
            SetUseText();
        }
    }

    // Called after OnActiveValueChanged event is triggered
    public void UpdatePrefabActiveStatus(GameObject myClickedPrefab)
    {
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            isPrefabActive = equipment.IsActive;
            ChangeUseButton();
        }
    }

    // Called after OnStatusChanged delegate event is invoked
    public void AllSafe()
    {
        isAllEquipSafe = true;
        Debug.Log("We're all safe!");
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
            replaceButton.SetActive(true);
        }
        else if(isPrefabSafe == true)
        {
            myInspectText.text = currentPrefabDescrSafe;
            replaceButton.SetActive(false);
        }
    }

    // Called at end of UpdatePrefabActiveStatus function
    // Changes UseButton text from activate to deactivate depending on value of isPrefabActive value
    // Requires second call in SetCurrentPrefabValues function to work properly
    public void ChangeUseButton()
    {
        if(isPrefabActive == true)
        {
            myUseButtonText.text = "Deactivate";
        }
        else if(isPrefabActive == false)
        {
            myUseButtonText.text = "Activate";
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
    }

    public void AddBackgroundBlur()
    {
        blurLayer.SetActive(true);
    }

    public void RemoveBlur()
    {
        blurLayer.SetActive(false);
    }

    public void ReplaceButtonClick()
    {
        OnInspectMenuActivate?.Invoke();
    }

    public void ExitBackButtonClick()
    {
        OnInspectMenuDeactivate?.Invoke();
    }

    public void UseButtonClick()
    {
        OnUseMenuActivate?.Invoke();
    }
}
