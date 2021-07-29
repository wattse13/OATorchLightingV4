using System.Collections;
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
        SafetyStateController.OnStatusChanged += UpdatePrefabSafetyStatus;
    }
    private void OnDisable()
    {
        // GameEvents.OnMessageSent -= SetCurrentPrefab;
        GameEvents.OnMessageSent -= SetCurrentPrefabValues;
        SafetyStateController.OnStatusChanged -= UpdatePrefabSafetyStatus;
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
            isPrefabSafe = equipment.IsSafe;
        }
    }

    public void UpdatePrefabSafetyStatus(GameObject myClickedPrefab)
    {
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            isPrefabSafe = equipment.IsSafe;
        }
    }

    //public void ValueTest()
    //{
    //    Debug.Log(isPrefabSafe);
    //    Debug.Log(currentPrefabName);
    //    // Debug.Log(currentPrefabDescrUnsafe);
    //}

    public void SetInspectText()
    {
        myInspectTitle.text = currentPrefabName;

        if(isPrefabSafe == false)
        {
            myInspectText.text = currentPrefabDescrUnsafe;
        }
        else if(isPrefabSafe == true)
        {
            myInspectText.text = currentPrefabDescrSafe;
        }
    }

    public void ChangeInspectText()
    {
        if(isPrefabSafe == true)
        {
            myInspectText.text = currentPrefabDescrSafe;
        }
    }

    // Want to remove Replace button after safetyStatus has been changed
    // Doesn't currently work
    public void ChangeInspectMenuButtons()
    {
        if(isPrefabSafe == false)
        {
            return;
        }
        else if(isPrefabSafe == true)
        {
            replaceButton.SetActive(false);
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
